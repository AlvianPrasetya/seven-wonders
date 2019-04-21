public class GainPointsIfCardCountEqualsOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amount;
	public CardType cardType;
	public int targetCount;

    public override void Effect(Player player) {
		GainPointsResolver.Count count = () => {
			int playerCardCount = player.BuiltCardsByType[cardType].Count;
			return (playerCardCount == targetCount) ? amount : 0;
		};

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}

}
