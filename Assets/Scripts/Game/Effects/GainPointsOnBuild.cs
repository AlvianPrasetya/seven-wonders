public class GainPointsOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amount;

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, () => { return amount; }),
			Priority.GainPoints
		);
	}

}
