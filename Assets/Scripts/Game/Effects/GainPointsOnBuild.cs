public class GainPointsOnBuild : OnBuildEffect {

	private const int Priority = 2;

	public PointType pointType;
	public int amount;

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, amount),
			Priority
		);
	}

}
