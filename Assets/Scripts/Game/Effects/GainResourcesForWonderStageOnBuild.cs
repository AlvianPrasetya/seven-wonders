using System.Collections.Generic;

public class GainResourcesForWonderStageOnBuild : OnBuildEffect {

	public bool lumber;
	public bool ore;
	public bool clay;
	public bool stone;
	public bool loom;
	public bool glassworks;
	public bool press;

	public override void Effect(Player player) {
		List<ResourceType> resources = new List<ResourceType>();
		if (lumber) {
			resources.Add(ResourceType.Lumber);
		}
		if (ore) {
			resources.Add(ResourceType.Ore);
		}
		if (clay) {
			resources.Add(ResourceType.Clay);
		}
		if (stone) {
			resources.Add(ResourceType.Stone);
		}
		if (loom) {
			resources.Add(ResourceType.Loom);
		}
		if (glassworks) {
			resources.Add(ResourceType.Glassworks);
		}
		if (press) {
			resources.Add(ResourceType.Press);
		}

		player.AddConditionalResource(
			new ConditionalResource(
				delegate(Player buildingPlayer, Card cardToBuild) {
					return new Resource[0];
				},
				delegate(Player buildingPlayer, WonderStage stageToBuild) {
					return new Resource[] {
						new Resource(false, resources.ToArray())
					};
				}
			)
		);
	}

}
