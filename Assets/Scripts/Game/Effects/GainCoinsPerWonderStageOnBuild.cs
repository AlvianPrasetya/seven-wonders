public class GainCoinsPerWonderStageOnBuild : OnBuildEffect {

	public int amountPerWonderStage;
	public Target countTarget;

	public override void Effect(Player player) {
		GainCoinsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return amountPerWonderStage * player.Wonder.BuiltStagesCount;
				};
				break;
			case Target.Neighbours:
				count = () => {
					return amountPerWonderStage * (
						player.Neighbours[Direction.West].Wonder.BuiltStagesCount +
						player.Neighbours[Direction.East].Wonder.BuiltStagesCount
					);
				};
				break;
			case Target.Neighbourhood:
				count = () => {
					return amountPerWonderStage * (
						player.Wonder.BuiltStagesCount +
						player.Neighbours[Direction.West].Wonder.BuiltStagesCount +
						player.Neighbours[Direction.East].Wonder.BuiltStagesCount
					);
				};
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
		GameManager.Instance.EnqueueResolver(
			new GainCoinsResolver(player, count),
			Priority.GainCoins
		);
	}

}
