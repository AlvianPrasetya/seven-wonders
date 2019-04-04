public class GainPointsPerCardOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amountPerCard;
	public CardType cardType;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return amountPerCard * player.BuiltCardsByType[cardType].Count;
				};
				break;
			case Target.Neighbours:
				count = () => {
					return amountPerCard * (
						player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count +
						player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count
					);
				};
				break;
			case Target.Neighbourhood:
				count = () => {
					return amountPerCard * (
						player.BuiltCardsByType[cardType].Count +
						player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count +
						player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count
					);
				};
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}

}
