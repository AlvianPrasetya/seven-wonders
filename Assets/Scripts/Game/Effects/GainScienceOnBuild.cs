using System.Collections.Generic;

public class GainScienceOnBuild : OnBuildEffect {

	public bool produced;

	public bool compass;
	public bool tablet;
	public bool gear;

	public override void Effect(Player player) {
		List<Science> sciences = new List<Science>();
		if (compass) {
			sciences.Add(Science.Compass);
		}
		if (tablet) {
			sciences.Add(Science.Tablet);
		}
		if (gear) {
			sciences.Add(Science.Gear);
		}

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(
				player,
				PointType.Scientific,
				() => {
					return player.AddScience(new ScienceOptions(produced, sciences.ToArray()));
				}
			),
			Priority.GainPoints
		);
	}

}
