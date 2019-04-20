public class GainPointsIfCoinsMoreThanNeighboursOnBuild : OnBuildEffect {
	public PointType pointType;
	public int victoryPoints;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = () => {
			int playerCoinCount = player.bank.Count;
			int westNeighbourCoinCount = player.Neighbours[Direction.West].bank.Count;
			int eastNeighbourCoinCount = player.Neighbours[Direction.East].bank.Count;

			return  ((playerCoinCount > westNeighbourCoinCount) &&  
					(playerCoinCount > eastNeighbourCoinCount)) ? victoryPoints : 0;
		};

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}
}