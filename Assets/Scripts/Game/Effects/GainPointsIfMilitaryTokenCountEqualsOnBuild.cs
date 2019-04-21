public class GainPointsIfMilitaryTokenCountEqualsOnBuild : OnBuildEffect {
    public PointType pointType;
	public int rewardPoints;
	public MilitaryTokenType militaryTokenType;
	public int tokenCount;
    public override void Effect(Player player) {
		GainPointsResolver.Count count = () => {
			int playerMilitaryTokenCount = player.militaryTokenDisplay.Count(militaryTokenType);

			return  playerMilitaryTokenCount == tokenCount ? rewardPoints : 0;
		};

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}
}
