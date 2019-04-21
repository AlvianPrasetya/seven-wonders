using System;
using System.Collections.Generic;

public class GainComplementaryResourceOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		player.AddConditionalResource(
			new ConditionalResource(
				delegate(Player buildingPlayer, Card cardToBuild) {
					HashSet<ResourceType> unownedResourceTypes = new HashSet<ResourceType>();
					foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) {
						unownedResourceTypes.Add(resourceType);
					}

					foreach (Resource producedResource in buildingPlayer.ProducedResources) {
						foreach (ResourceType resourceType in producedResource.ResourceTypes) {
							unownedResourceTypes.Remove(resourceType);
						}
					}

					if (unownedResourceTypes.Count == 0) {
						return new Resource[0];
					}

					ResourceType[] resourceTypes = new ResourceType[unownedResourceTypes.Count];
					unownedResourceTypes.CopyTo(resourceTypes);
					return new Resource[] {
						new Resource(false, resourceTypes)
					};
				},
				delegate(Player buildingPlayer, WonderStage stageToBuild) {
					HashSet<ResourceType> unownedResourceTypes = new HashSet<ResourceType>();
					foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) {
						unownedResourceTypes.Add(resourceType);
					}

					foreach (Resource producedResource in buildingPlayer.ProducedResources) {
						foreach (ResourceType resourceType in producedResource.ResourceTypes) {
							unownedResourceTypes.Remove(resourceType);
						}
					}

					if (unownedResourceTypes.Count == 0) {
						return new Resource[0];
					}

					ResourceType[] resourceTypes = new ResourceType[unownedResourceTypes.Count];
					unownedResourceTypes.CopyTo(resourceTypes);
					return new Resource[] {
						new Resource(false, resourceTypes)
					};
				}
			)
		);
	}

}
