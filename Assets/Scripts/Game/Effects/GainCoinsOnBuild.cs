public class GainCoinsOnBuild : OnBuildEffect {

	public Target target;
	public int amount;

	public override void Effect(Player player) {
		GainCoinsResolver.Count count = () => { return amount; };
		switch (target) {
			case Target.Self:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player, count),
					Priority.GainCoins
				);
				break;
			case Target.Neighbours:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.West], count),
					Priority.GainCoins
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.East], count),
					Priority.GainCoins
				);
				break;
			case Target.Neighbourhood:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player, count),
					Priority.GainCoins
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.West], count),
					Priority.GainCoins
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.East], count),
					Priority.GainCoins
				);
				break;
			case Target.Others:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					if (targetPlayer == player) {
						continue;
					}

					GameManager.Instance.EnqueueResolver(
						new GainCoinsResolver(targetPlayer, count),
						Priority.GainCoins
					);
				}
				break;
			case Target.Everyone:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					GameManager.Instance.EnqueueResolver(
						new GainCoinsResolver(targetPlayer, count),
						Priority.GainCoins
					);
				}
				break;
		}
	}

}
