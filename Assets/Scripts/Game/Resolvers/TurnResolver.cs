using Photon.Pun;
using System.Collections;

public class TurnResolver : IResolvable {

	public static Player DoubleTurnPlayer { private get; set; }

	private DeckType sourceDeck;
	private DeckType targetDeck;
	private Direction direction;
	private bool targetSelf;

	public TurnResolver(DeckType sourceDeck, DeckType targetDeck, Direction direction, bool targetSelf = false) {
		this.sourceDeck = sourceDeck;
		this.targetDeck = targetDeck;
		this.direction = direction;
		this.targetSelf = targetSelf;
	}

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(sourceDeck, direction),
			Priority.PlayHand
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		if (PhotonNetwork.IsMasterClient) {
			foreach (Bot bot in GameManager.Instance.Bots) {
				GameManager.Instance.EnqueueResolver(
					new DecideBotActionResolver(bot),
					Priority.PlayHand
				);
			}
		}
		GameManager.Instance.EnqueueResolver(
			new DecideActionResolver(GameOptions.DecideTime),
			Priority.PlayHand
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		GameManager.Instance.EnqueueResolver(new PerformActionResolver(), Priority.PlayHand);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(), Priority.PlayHand);
		
		yield return CheckDoubleTurn();
		
		GameManager.Instance.EnqueueResolver(
			new UnloadHandResolver(targetDeck, direction, targetSelf),
			(targetDeck == DeckType.Discard) ? Priority.DiscardLastHand : Priority.PlayHand
		);
		
		yield return null;
	}

	private IEnumerator CheckDoubleTurn() {
		if (targetDeck == DeckType.Discard && DoubleTurnPlayer != null) {
			if (PhotonNetwork.IsMasterClient && DoubleTurnPlayer.GetType() == typeof(Bot)) {
				GameManager.Instance.EnqueueResolver(
					new DecideBotActionResolver((Bot)DoubleTurnPlayer),
					Priority.PlayHand
				);
			} else if (DoubleTurnPlayer == GameManager.Instance.Player) {
				// This is the player with double turn, enqueue DecideActionResolver
				GameManager.Instance.EnqueueResolver(
					new DecideActionResolver(GameOptions.DecideTime),
					Priority.PlayHand
				);
			}
			GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

			GameManager.Instance.EnqueueResolver(new PerformActionResolver(DoubleTurnPlayer), Priority.PlayHand);
			GameManager.Instance.EnqueueResolver(new EffectActionResolver(DoubleTurnPlayer), Priority.PlayHand);
		}
		
		yield return null;
	}

}
