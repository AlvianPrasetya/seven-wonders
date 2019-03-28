using System.Collections.Generic;

public class GainResourcesOnBuild : OnBuildEffect {

	public bool lumber;
	public bool ore;
	public bool clay;
	public bool stone;
	public bool loom;
	public bool glassworks;
	public bool press;

	public override void Effect(Player player) {
		List<Resource> resources = new List<Resource>();
		if (lumber) {
			resources.Add(Resource.Lumber);
		}
		if (ore) {
			resources.Add(Resource.Ore);
		}
		if (clay) {
			resources.Add(Resource.Clay);
		}
		if (stone) {
			resources.Add(Resource.Stone);
		}
		if (loom) {
			resources.Add(Resource.Loom);
		}
		if (glassworks) {
			resources.Add(Resource.Glassworks);
		}
		if (press) {
			resources.Add(Resource.Press);
		}

		player.AddResource(new ResourceOptions(resources.ToArray()));
	}

}
