public class GainCoinsPerCardOnBuild : OnBuildEffect {

	public int amountPerCard;
	public CardType cardType;
	public Target countTarget;

	public override void Effect(Player player) {
		int priority;
		if (countTarget == Target.Self) {
			priority = 5;
		} else {
			priority = 4;
		}

		GameManager.Instance.EnqueueResolver(
			new GainCoinsPerCardResolver(
				player,
				amountPerCard,
				cardType,
				countTarget
			),
			priority
		);
	}

}
