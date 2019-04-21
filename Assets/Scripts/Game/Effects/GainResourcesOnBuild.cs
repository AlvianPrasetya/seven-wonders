using System.Collections.Generic;

public class GainResourcesOnBuild : OnBuildEffect {

	public bool produced;

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

		player.AddResource(new Resource(produced, resources.ToArray()));
	}

}
