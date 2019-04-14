using System.Collections.Generic;

public class GainResourcesForCardOnBuild : OnBuildEffect {

	public CardType cardType;

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

		player.AddConditionalResource(
			new ConditionalResourceOptions(
				delegate(Player buildingPlayer, Card cardToBuild) {
					if (cardToBuild.cardType == cardType) {
						return new ResourceOptions[] {
							new ResourceOptions(false, resources.ToArray())
						};
					}

					return new ResourceOptions[0];
				},
				delegate(Player buildingPlayer, WonderStage stageToBuild) {
					return new ResourceOptions[0];
				}
			)
		);
	}

}
