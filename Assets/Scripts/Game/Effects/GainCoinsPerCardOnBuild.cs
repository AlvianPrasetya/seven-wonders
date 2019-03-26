public class GainCoinsPerCardOnBuild : OnBuildEffect {

	public int amountPerCard;
	public CardType cardType;
	public Target countTarget;

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new GainCoinsPerCardResolver(
				player,
				amountPerCard,
				cardType,
				countTarget
			),
			4
		);
	}

}
