public class GainPointsIfCardCountEqualsOnBuild : OnBuildEffect {
	public PointType pointType;
	public int rewardPoints;
	public CardType cardType;
	public int cardCount;
    public override void Effect(Player player) {
		GainPointsResolver.Count count = () => {
			int playerCardCount = player.BuiltCardsByType[cardType].Count;

			return  playerCardCount == cardCount ? rewardPoints : 0;
		};

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}
}
