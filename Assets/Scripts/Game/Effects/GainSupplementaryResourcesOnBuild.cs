using System.Collections.Generic;

public class GainSupplementaryResourcesOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		player.AddConditionalResource(
			new ConditionalResource(
				delegate(Player buildingPlayer, Card cardToBuild) {
					HashSet<ResourceType> ownedResourceTypes = new HashSet<ResourceType>();
					foreach (Resource producedResource in buildingPlayer.ProducedResources) {
						foreach (ResourceType resourceType in producedResource.ResourceTypes) {
							ownedResourceTypes.Add(resourceType);
						}
					}

					if (ownedResourceTypes.Count == 0) {
						return new Resource[0];
					}

					ResourceType[] resourceTypes = new ResourceType[ownedResourceTypes.Count];
					ownedResourceTypes.CopyTo(resourceTypes);
					return new Resource[] {
						new Resource(false, resourceTypes)
					};
				},
				delegate(Player buildingPlayer, WonderStage stageToBuild) {
					HashSet<ResourceType> ownedResourceTypes = new HashSet<ResourceType>();
					foreach (Resource producedResource in buildingPlayer.ProducedResources) {
						foreach (ResourceType resourceType in producedResource.ResourceTypes) {
							ownedResourceTypes.Add(resourceType);
						}
					}

					if (ownedResourceTypes.Count == 0) {
						return new Resource[0];
					}

					ResourceType[] resourceTypes = new ResourceType[ownedResourceTypes.Count];
					ownedResourceTypes.CopyTo(resourceTypes);
					return new Resource[] {
						new Resource(false, resourceTypes)
					};
				}
			)
		);
	}

}
