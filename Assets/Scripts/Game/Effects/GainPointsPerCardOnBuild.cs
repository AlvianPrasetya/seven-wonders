public class GainPointsPerCardOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amountPerCard;
	public CardType cardType;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsPerCountResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count += () => player.BuiltCardsByType[cardType].Count;
				break;
			case Target.Neighbours:
				count += () => player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count;
				count += () => player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count;
				break;
			case Target.Neighbourhood:
				count += () => player.BuiltCardsByType[cardType].Count;
				count += () => player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count;
				count += () => player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count;
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
		GameManager.Instance.EnqueueResolver(
			new GainPointsPerCountResolver(player, pointType, amountPerCard, count),
			Priority.GainPoints
		);
	}

}
