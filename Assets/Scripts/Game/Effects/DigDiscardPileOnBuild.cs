public class DigDiscardPileOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new DigDiscardPileResolver(player),
			Priority.DigDiscardPile
		);
	}

}
