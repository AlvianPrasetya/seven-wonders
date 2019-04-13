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

		List<KeyValuePair<Direction, Resource>> discountedPurchases =
			new List<KeyValuePair<Direction, Resource>>();
		if (lumber) {
			discountedPurchases.Add(new KeyValuePair<Direction, Resource>(direction, Resource.Lumber));
		}

		if (ore) {
			discountedPurchases.Add(new KeyValuePair<Direction, Resource>(direction, Resource.Ore));
		}

		if (clay) {
			discountedPurchases.Add(new KeyValuePair<Direction, Resource>(direction, Resource.Clay));
		}

		if (stone) {
			discountedPurchases.Add(new KeyValuePair<Direction, Resource>(direction, Resource.Stone));
		}

		if (loom) {
			discountedPurchases.Add(new KeyValuePair<Direction, Resource>(direction, Resource.Loom));
		}

		if (glassworks) {
			discountedPurchases.Add(new KeyValuePair<Direction, Resource>(direction, Resource.Glassworks));
		}

		if (press) {
			discountedPurchases.Add(new KeyValuePair<Direction, Resource>(direction, Resource.Press));
		}

		foreach (KeyValuePair<Direction, Resource> discountedPurchase in discountedPurchases) {
			player.ResourceBuyCosts[discountedPurchase] = GameOptions.DiscountedBuyCost;
		}
	}

}
