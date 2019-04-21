using System.Collections.Generic;

public class CopyScienceOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(
				player,
				PointType.Scientific,
				() => {
					return player.AddScience(new Science(false, new ScienceType[] {
						ScienceType.Copy
					}));
				}
			),
			Priority.GainPointsDelayed
		);
	}

}
