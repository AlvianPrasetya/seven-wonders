public class GainPointsIfCardsMoreThanNeighboursOnBuild : OnBuildEffect {
	public PointType pointType;
	public int victoryPoints;
	public CardType cardType;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = () => {
			int playerCardCount = player.BuiltCardsByType[cardType].Count;
			int westNeighbourCardCount = player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count;
			int eastNeighbourCardCount = player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count;

			return  ((playerCardCount > westNeighbourCardCount) &&  
					(playerCardCount > eastNeighbourCardCount)) ? victoryPoints : 0;
		};

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}
}