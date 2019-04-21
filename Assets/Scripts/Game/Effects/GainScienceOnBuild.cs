using System.Collections.Generic;

public class GainScienceOnBuild : OnBuildEffect {

	public bool produced;

	public bool compass;
	public bool tablet;
	public bool gear;

	public override void Effect(Player player) {
		List<ScienceType> sciences = new List<ScienceType>();
		if (compass) {
			sciences.Add(ScienceType.Compass);
		}
		if (tablet) {
			sciences.Add(ScienceType.Tablet);
		}
		if (gear) {
			sciences.Add(ScienceType.Gear);
		}

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(
				player,
				PointType.Scientific,
				() => {
					return player.AddScience(new Science(produced, sciences.ToArray()));
				}
			),
			Priority.GainPoints
		);
	}

}
