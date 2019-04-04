using Photon.Pun;

public class DigDiscardPileOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new UnloadHandResolver(DeckType.Swap, Direction.East, player),
			Priority.DigDiscardPile
		);
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(DeckType.Discard, Direction.East, player),
			Priority.DigDiscardPile
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.DigDiscardPile);

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
			new UnloadHandResolver(DeckType.Discard, Direction.East, player),
			Priority.DigDiscardPile
		);
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(DeckType.Swap, Direction.East, player),
			Priority.DigDiscardPile
		);
	}

}