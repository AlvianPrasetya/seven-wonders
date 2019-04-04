using System;

public class GainPointsPerResourceOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amountPerResource;
	public Resource resource;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsPerCountResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count += () => { return player.CountResource(resource); };
				break;
			case Target.Neighbours:
				count += () => { return player.Neighbours[Direction.West].CountResource(resource); };
				count += () => { return player.Neighbours[Direction.East].CountResource(resource); };
				break;
			case Target.Neighbourhood:
				count += () => { return player.CountResource(resource); };
				count += () => { return player.Neighbours[Direction.West].CountResource(resource); };
				count += () => { return player.Neighbours[Direction.East].CountResource(resource); };
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
		GameManager.Instance.EnqueueResolver(
			new GainPointsPerCountResolver(player, pointType, amountPerResource, count),
			Priority.GainPoints
		);
	}

}
