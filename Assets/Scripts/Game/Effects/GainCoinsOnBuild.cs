public class GainCoinsOnBuild : OnBuildEffect {

	private const int Priority = 6;

	public Target target;
	public int amount;

	public override void Effect(Player player) {
		switch (target) {
			case Target.Self:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player, amount),
					Priority
				);
				break;
			case Target.Neighbours:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.West], amount),
					Priority
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.East], amount),
					Priority
				);
				break;
			case Target.Neighbourhood:
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player, amount),
					Priority
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.West], amount),
					Priority
				);
				GameManager.Instance.EnqueueResolver(
					new GainCoinsResolver(player.Neighbours[Direction.East], amount),
					Priority
				);
				break;
			case Target.Others:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					if (targetPlayer == player) {
						continue;
					}

					GameManager.Instance.EnqueueResolver(
						new GainCoinsResolver(targetPlayer, amount),
						Priority
					);
				}
				break;
			case Target.Everyone:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					GameManager.Instance.EnqueueResolver(
						new GainCoinsResolver(targetPlayer, amount),
						Priority
					);
				}
				break;
		}
	}

}
