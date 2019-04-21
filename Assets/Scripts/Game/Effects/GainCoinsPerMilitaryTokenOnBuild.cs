public class GainCoinsPerMilitaryTokenOnBuild : OnBuildEffect {

	public int amountPerMilitaryToken;
	public MilitaryTokenType militaryTokenType;
	public Target countTarget;

	public override void Effect(Player player) {
		GainCoinsResolver.Count count = null;
		switch (countTarget) {
			case Target.Self:
				count = () => {
					return amountPerMilitaryToken * player.militaryTokenDisplay.Count(militaryTokenType);
				};
				break;
			case Target.Neighbours:
				count = () => {
					return amountPerMilitaryToken * (
						player.Neighbours[Direction.West].militaryTokenDisplay.Count(militaryTokenType) +
						player.Neighbours[Direction.East].militaryTokenDisplay.Count(militaryTokenType)
					);
				};
				break;
			case Target.Neighbourhood:
				count = () => {
					return amountPerMilitaryToken * (
						player.militaryTokenDisplay.Count(militaryTokenType) +
						player.militaryTokenDisplay.Count(militaryTokenType) +
						player.militaryTokenDisplay.Count(militaryTokenType)
					);
				};
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
		GameManager.Instance.EnqueueResolver(
			new GainCoinsResolver(player, count),
			Priority.GainCoins
		);
	}

}
