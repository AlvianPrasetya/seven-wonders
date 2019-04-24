using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class LastTurnResolver : IResolvable {

	private class ExtraTurnResolver : IResolvable {

		public IEnumerator Resolve() {
			GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayExtraHand);

			foreach (Player extraTurnPlayer in extraTurnPlayers) {
				if (extraTurnPlayer.GetType() == typeof(Bot)) {
					GameManager.Instance.EnqueueResolver(
						new DecideBotActionResolver((Bot)extraTurnPlayer),
						Priority.PlayExtraHand
					);
				} else if (extraTurnPlayer == GameManager.Instance.Player) {
					GameManager.Instance.EnqueueResolver(
						new DecideActionResolver(GameOptions.DecideTime),
						Priority.PlayExtraHand
					);
				}
			}
			GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayExtraHand);

			GameManager.Instance.EnqueueResolver(
				new PerformActionResolver(extraTurnPlayers.ToArray()),
				Priority.PlayExtraHand
			);
			GameManager.Instance.EnqueueResolver(
				new EffectActionResolver(extraTurnPlayers.ToArray()),
				Priority.PlayExtraHand
			);
			
			yield return null;
		}

	}

	private static List<Player> extraTurnPlayers = new List<Player>();

	private DeckType? sourceDeck;
	private DeckType? targetDeck;
	private Direction direction;
	private bool targetSelf;

	public LastTurnResolver(DeckType? sourceDeck, DeckType? targetDeck, Direction direction, bool targetSelf = false) {
		this.sourceDeck = sourceDeck;
		this.targetDeck = targetDeck;
		this.direction = direction;
		this.targetSelf = targetSelf;
	}

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		if (sourceDeck != null) {
			GameManager.Instance.EnqueueResolver(
				new UnloadDeckResolver(sourceDeck ?? default(DeckType), direction),
				Priority.PlayHand
			);
		}

		foreach (Bot bot in GameManager.Instance.Bots) {
			GameManager.Instance.EnqueueResolver(
				new DecideBotActionResolver(bot),
				Priority.PlayHand
			);
		}
		GameManager.Instance.EnqueueResolver(
			new DecideActionResolver(GameOptions.DecideTime),
			Priority.PlayHand
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		GameManager.Instance.EnqueueResolver(new PerformActionResolver(), Priority.PlayHand);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(), Priority.PlayHand);
		
		GameManager.Instance.EnqueueResolver(new ExtraTurnResolver(), Priority.PlayHand);

		if (targetDeck != null) {
			GameManager.Instance.EnqueueResolver(
				new UnloadHandResolver(targetDeck ?? default(DeckType), direction, targetSelf), Priority.PlayHand
			);
		}
		
		yield return null;
	}

	public static void AddExtraTurnPlayer(Player player) {
		extraTurnPlayers.Add(player);
	}

}
