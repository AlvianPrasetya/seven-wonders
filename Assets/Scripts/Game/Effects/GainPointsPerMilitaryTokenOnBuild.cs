using System;

public class GainPointsPerMilitaryTokenOnBuild : OnBuildEffect {

	public PointType pointType;
	public int baseAmount;
	public int amountPerMilitaryToken;
	public MilitaryTokenType militaryTokenType;
	public Age[] ages;
	public Target countTarget;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return Math.Max(
						baseAmount + amountPerMilitaryToken * countMilitaryTokenOf(player),
						0
					);
				};
				break;
			case Target.Neighbours:
				count = () => {
					return Math.Max(
						baseAmount + amountPerMilitaryToken * (
							countMilitaryTokenOf(player.Neighbours[Direction.West]) +
							countMilitaryTokenOf(player.Neighbours[Direction.East])
						),
						0
					);
				};
				break;
			case Target.Neighbourhood:
				count = () => {
					return Math.Max(
						baseAmount + amountPerMilitaryToken * (
							countMilitaryTokenOf(player) +
							countMilitaryTokenOf(player.Neighbours[Direction.West]) +
							countMilitaryTokenOf(player.Neighbours[Direction.East])
						),
						0
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

	private int countMilitaryTokenOf(Player player) {
		if (militaryTokenType != MilitaryTokenType.Victory) {
			return player.militaryTokenDisplay.Count(militaryTokenType);
		}

		int count = 0;
		MilitaryTokenSlot[] militaryTokenSlots = player.militaryTokenDisplay.militaryTokenSlots;

		foreach(Age age in ages) {
			switch (age) {
				case Age.Age1:
					count += Array.FindAll(militaryTokenSlots, slot => slot.Element.points == 1).Length;
					break;
				case Age.Age2:
					count += Array.FindAll(militaryTokenSlots, slot => slot.Element.points == 3).Length;
					break;
				case Age.Age3:
					count += Array.FindAll(militaryTokenSlots, slot => slot.Element.points == 5).Length;
					break;
			}
		}

		return count;
	}

}
