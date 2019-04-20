using Photon.Pun;
using System.Collections;

public class DigDiscardPileResolver : IResolvable {

	private Player player;

	public DigDiscardPileResolver(Player player) {
		this.player = player;
	}

	public IEnumerator Resolve() {
		if (GameManager.Instance.discardPile.Count == 0) {
			// Discard pile is empty, this effect is nullified
			yield break;
		}
		
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.DigDiscardPile);

		GameManager.Instance.EnqueueResolver(
			new UnloadHandResolver(DeckType.Swap, Direction.East, true, player),
			Priority.DigDiscardPile
		);
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(DeckType.Discard, Direction.East, player),
			Priority.DigDiscardPile
		);

		if (player.GetType() == typeof(Bot) && PhotonNetwork.IsMasterClient) {
			// This player is a bot, enqueue decide bot action resolver on master client
			GameManager.Instance.EnqueueResolver(
				new DecideBotActionResolver((Bot)player),
				Priority.DigDiscardPile
			);
		} else if (player == GameManager.Instance.Player) {
			GameManager.Instance.EnqueueResolver(
				new DecideActionResolver(GameOptions.DecideTime),
				Priority.DigDiscardPile
			);
		}
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.DigDiscardPile);
		
		GameManager.Instance.EnqueueResolver(new PerformActionResolver(player), Priority.DigDiscardPile);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(player), Priority.DigDiscardPile);
		GameManager.Instance.EnqueueResolver(
			new UnloadHandResolver(DeckType.Discard, Direction.East, true, player),
			Priority.DigDiscardPile
		);
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(DeckType.Swap, Direction.East, player),
			Priority.DigDiscardPile
		);
	}

}
