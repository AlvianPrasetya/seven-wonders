using System.Collections.Generic;

public class GreatWall : Wonder {

	public override WonderStage[] GetBuildableStages() {
		List<WonderStage> buildableStages = new List<WonderStage>();
		foreach (WonderStage wonderStage in wonderStages) {
			if (!wonderStage.IsBuilt) {
				buildableStages.Add(wonderStage);
			}
		}

		return buildableStages.ToArray();
	}

}
