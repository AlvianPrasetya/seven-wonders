public class LoseCoinsOnBuild : OnBuildEffect {

	public Target target;
	public int amount;

	public override void Effect(Player player) {
		LoseCoinsResolver.Count count = () => {
			if (amount == Amount.Age) {
				return (int)GameManager.Instance.CurrentAge;
			}

			return amount;
		};
		switch (target) {
			case Target.Self:
				GameManager.Instance.EnqueueResolver(
					new LoseCoinsResolver(player, count),
					Priority.LoseCoins
				);
				break;
			case Target.Neighbours:
				GameManager.Instance.EnqueueResolver(
					new LoseCoinsResolver(player.Neighbours[Direction.West], count),
					Priority.LoseCoins
				);
				GameManager.Instance.EnqueueResolver(
					new LoseCoinsResolver(player.Neighbours[Direction.East], count),
					Priority.LoseCoins
				);
				break;
			case Target.Neighbourhood:
				GameManager.Instance.EnqueueResolver(
					new LoseCoinsResolver(player, count),
					Priority.LoseCoins
				);
				GameManager.Instance.EnqueueResolver(
					new LoseCoinsResolver(player.Neighbours[Direction.West], count),
					Priority.LoseCoins
				);
				GameManager.Instance.EnqueueResolver(
					new LoseCoinsResolver(player.Neighbours[Direction.East], count),
					Priority.LoseCoins
				);
				break;
			case Target.Others:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					if (targetPlayer == player) {
						continue;
					}

					GameManager.Instance.EnqueueResolver(
						new LoseCoinsResolver(targetPlayer, count),
						Priority.LoseCoins
					);
				}
				break;
			case Target.Everyone:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					GameManager.Instance.EnqueueResolver(
						new LoseCoinsResolver(targetPlayer, count),
						Priority.LoseCoins
					);
				}
				break;
		}
	}

}
