using System;

public class GainPointsPerResourceOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amountPerResource;
	public Resource resource;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return amountPerResource * player.CountResource(resource);
				};
				break;
			case Target.Neighbours:
				count = () => {
					return amountPerResource * (
						player.Neighbours[Direction.West].CountResource(resource) +
						player.Neighbours[Direction.East].CountResource(resource)
					);
				};
				break;
			case Target.Neighbourhood:
				count = () => {
					return amountPerResource * (
						player.CountResource(resource) +
						player.Neighbours[Direction.West].CountResource(resource) +
						player.Neighbours[Direction.East].CountResource(resource)
					);
				};
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
