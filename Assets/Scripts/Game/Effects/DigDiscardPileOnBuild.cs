using Photon.Pun;

public class DigDiscardPileOnBuild : OnBuildEffect {

	private const int Priority = 5;

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new UnloadHandResolver(DeckType.Swap, Direction.East, player),
			Priority
		);
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(DeckType.Discard, Direction.East, player),
			Priority
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority);

		if (player.GetType() == typeof(Bot)) {
			// This player is a bot
			if (PhotonNetwork.IsMasterClient) {
				// Enqueue decide bot action resolver on master client
				GameManager.Instance.EnqueueResolver(
					new DecideBotActionResolver((Bot)player),
					Priority
				);
			}
		} else if (player == GameManager.Instance.Player) {
			GameManager.Instance.EnqueueResolver(
				new DecideActionResolver(GameOptions.DecideTime),
				Priority
			);
		}
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority);
		
		GameManager.Instance.EnqueueResolver(new PerformActionResolver(player), Priority);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(player), Priority);
		GameManager.Instance.EnqueueResolver(
			new UnloadHandResolver(DeckType.Discard, Direction.East, player),
			Priority
		);
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(DeckType.Swap, Direction.East, player),
			Priority
		);
	}

}
