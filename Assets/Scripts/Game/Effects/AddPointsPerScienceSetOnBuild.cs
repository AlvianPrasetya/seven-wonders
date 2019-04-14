public class AddPointsPerScienceSetOnBuild : OnBuildEffect {

	public int extraPointsPerSet;

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(
				player,
				PointType.Scientific,
				() => {
					return player.AddPointsPerScienceSet(extraPointsPerSet);
				}
			),
			Priority.GainPoints
		);
	}

}
