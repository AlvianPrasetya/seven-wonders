public class GreatWall : Wonder {

	public override bool IsPlayable {
		set {
			foreach (WonderStage wonderStage in wonderStages) {
				wonderStage.IsPlayable = !wonderStage.IsBuilt && value;
			}
		}
	}

}
