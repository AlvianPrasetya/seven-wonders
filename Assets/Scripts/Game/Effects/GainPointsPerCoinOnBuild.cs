public class GainPointsPerCoinOnBuild : OnBuildEffect {

	public PointType pointType;
	public int coinsPerPoint;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return player.bank.Count / coinsPerPoint;
				};
				break;
			case Target.Neighbours:
				break;
			case Target.Neighbourhood:
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}

}
