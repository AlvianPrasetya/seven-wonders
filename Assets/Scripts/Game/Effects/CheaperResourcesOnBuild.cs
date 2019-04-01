using System.Collections.Generic;

public class CheaperResourcesOnBuild : OnBuildEffect {

	public Direction direction;
	public bool lumber;
	public bool ore;
	public bool clay;
	public bool stone;
	public bool loom;
	public bool glassworks;
	public bool press;


	public override void Effect(Player player) {
		Player neighbour = player.Neighbours[direction];

		List<Player.PlayerResource> playerResources = new List<Player.PlayerResource>();
		if (lumber) {
			playerResources.Add(new Player.PlayerResource(neighbour, Resource.Lumber));
		}

		if (ore) {
			playerResources.Add(new Player.PlayerResource(neighbour, Resource.Ore));
		}

		if (clay) {
			playerResources.Add(new Player.PlayerResource(neighbour, Resource.Clay));
		}

		if (stone) {
			playerResources.Add(new Player.PlayerResource(neighbour, Resource.Stone));
		}

		if (loom) {
			playerResources.Add(new Player.PlayerResource(neighbour, Resource.Loom));
		}

		if (glassworks) {
			playerResources.Add(new Player.PlayerResource(neighbour, Resource.Glassworks));
		}

		if (press) {
			playerResources.Add(new Player.PlayerResource(neighbour, Resource.Press));
		}

		foreach (Player.PlayerResource playerResource in playerResources) {
			player.ResourceBuyCosts[playerResource] = GameOptions.DiscountedBuyCost;
		}
	}

}
