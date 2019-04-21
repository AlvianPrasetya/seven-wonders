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

		List<KeyValuePair<Direction, ResourceType>> discountedPurchases =
			new List<KeyValuePair<Direction, ResourceType>>();
		if (lumber) {
			discountedPurchases.Add(new KeyValuePair<Direction, ResourceType>(direction, ResourceType.Lumber));
		}

		if (ore) {
			discountedPurchases.Add(new KeyValuePair<Direction, ResourceType>(direction, ResourceType.Ore));
		}

		if (clay) {
			discountedPurchases.Add(new KeyValuePair<Direction, ResourceType>(direction, ResourceType.Clay));
		}

		if (stone) {
			discountedPurchases.Add(new KeyValuePair<Direction, ResourceType>(direction, ResourceType.Stone));
		}

		if (loom) {
			discountedPurchases.Add(new KeyValuePair<Direction, ResourceType>(direction, ResourceType.Loom));
		}

		if (glassworks) {
			discountedPurchases.Add(new KeyValuePair<Direction, ResourceType>(direction, ResourceType.Glassworks));
		}

		if (press) {
			discountedPurchases.Add(new KeyValuePair<Direction, ResourceType>(direction, ResourceType.Press));
		}

		foreach (KeyValuePair<Direction, ResourceType> discountedPurchase in discountedPurchases) {
			player.ResourceBuyCosts[discountedPurchase] = GameOptions.DiscountedBuyCost;
		}
	}

}
