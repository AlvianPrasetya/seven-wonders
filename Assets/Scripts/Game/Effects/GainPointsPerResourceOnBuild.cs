using System;

public class GainPointsPerResourceOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amountPerResource;
	public ResourceType resourceType;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return amountPerResource * player.CountResource(resourceType);
				};
				break;
			case Target.Neighbours:
				count = () => {
					return amountPerResource * (
						player.Neighbours[Direction.West].CountResource(resourceType) +
						player.Neighbours[Direction.East].CountResource(resourceType)
					);
				};
				break;
			case Target.Neighbourhood:
				count = () => {
					return amountPerResource * (
						player.CountResource(resourceType) +
						player.Neighbours[Direction.West].CountResource(resourceType) +
						player.Neighbours[Direction.East].CountResource(resourceType)
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
