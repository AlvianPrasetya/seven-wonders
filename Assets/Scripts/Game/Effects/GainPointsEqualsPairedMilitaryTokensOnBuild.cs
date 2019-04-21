public class GainPointsEqualsPairedMilitaryTokensOnBuild : OnBuildEffect {
    public PointType pointType;

    public override void Effect(Player player) {
		GainPointsResolver.Count count = () => {
			int age1VictoryCount = 0;
			int age2VictoryCount = 0;
			int age3VictoryCount = 0;

			MilitaryTokenSlot[] militaryTokenSlots = player.militaryTokenDisplay.militaryTokenSlots;
			foreach (MilitaryTokenSlot slot in militaryTokenSlots) {
				MilitaryTokenType type = slot.Element.type;

				if (type != MilitaryTokenType.Victory) { continue; }

				int points = slot.Element.points;

				switch (points) {
					case 1:
						age1VictoryCount++;
						break;
					case 3:
						age2VictoryCount++;
						break;
					case 5:
						age3VictoryCount++;
						break;
					default:
						break;
				}
			};

			return (age1VictoryCount / 2 * 1) + (age2VictoryCount / 2 * 3) + (age3VictoryCount / 2 * 5);

		};

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}
}
