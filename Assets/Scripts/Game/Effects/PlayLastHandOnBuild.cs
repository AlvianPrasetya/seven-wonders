using Photon.Pun;

public class PlayLastHandOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		if (player.GetType() == typeof(Bot) && PhotonNetwork.IsMasterClient) {
			// This player is a bot, enqueue decide bot action resolver on master client
			GameManager.Instance.EnqueueResolver(
				new DecideBotActionResolver((Bot)player),
				Priority.PlayLastHand
			);
		} else if (player == GameManager.Instance.Player) {
			GameManager.Instance.EnqueueResolver(
				new DecideActionResolver(GameOptions.DecideTime),
				Priority.PlayLastHand
			);
		}
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayLastHand);

		GameManager.Instance.EnqueueResolver(new PerformActionResolver(player), Priority.PlayLastHand);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(player), Priority.PlayLastHand);
	}

}
