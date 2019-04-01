public class GainPointsPerWonderStageOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amountPerWonderStage;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsPerCountResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count += () => player.Wonder.BuiltStagesCount;
				break;
			case Target.Neighbours:
				count += () => player.Neighbours[Direction.West].Wonder.BuiltStagesCount;
				count += () => player.Neighbours[Direction.East].Wonder.BuiltStagesCount;
				break;
			case Target.Neighbourhood:
				count += () => player.Wonder.BuiltStagesCount;
				count += () => player.Neighbours[Direction.West].Wonder.BuiltStagesCount;
				count += () => player.Neighbours[Direction.East].Wonder.BuiltStagesCount;
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
		GameManager.Instance.EnqueueResolver(
			new GainPointsPerCountResolver(player, pointType, amountPerWonderStage, count), 2
		);
	}

}
