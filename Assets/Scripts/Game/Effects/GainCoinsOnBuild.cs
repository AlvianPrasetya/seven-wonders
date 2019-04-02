public class GainCoinsOnBuild : OnBuildEffect {

	public Target target;
	public int amount;

	public override void Effect(Player player) {
		switch (target) {
			case Target.Self:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player, amount),
					Priority.GainCoins
				);
				break;
			case Target.Neighbours:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.West], amount),
					Priority.GainCoins
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.East], amount),
					Priority.GainCoins
				);
				break;
			case Target.Neighbourhood:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player, amount),
					Priority.GainCoins
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.West], amount),
					Priority.GainCoins
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.East], amount),
					Priority.GainCoins
				);
				break;
			case Target.Others:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					if (targetPlayer == player) {
						continue;
					}

					GameManager.Instance.EnqueueResolver(
						new GainCoinsResolver(targetPlayer, amount),
						Priority.GainCoins
					);
				}
				break;
			case Target.Everyone:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					GameManager.Instance.EnqueueResolver(
						new GainCoinsResolver(targetPlayer, amount),
						Priority.GainCoins
					);
				}
				break;
		}
	}

}
