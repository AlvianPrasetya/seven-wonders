using UnityEngine;

public class GainPointsPerCardSetOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amountPerSet;
	public CardType[] cardTypesPerSet;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return amountPerSet * CountSets(player);
				};
				break;
			case Target.Neighbours:
				count = () => {
					return amountPerSet * (
						CountSets(player.Neighbours[Direction.West]) +
						CountSets(player.Neighbours[Direction.East])
					);
				};
				break;
			case Target.Neighbourhood:
				count = () => {
					return amountPerSet * (
						CountSets(player) +
						CountSets(player.Neighbours[Direction.West]) +
						CountSets(player.Neighbours[Direction.East])
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

	private int CountSets(Player player) {
		int numSets = int.MaxValue;
		foreach (CardType cardType in cardTypesPerSet) {
			numSets = Mathf.Min(numSets, player.BuiltCardsByType[cardType].Count);
		}

		return numSets;
	}

}
