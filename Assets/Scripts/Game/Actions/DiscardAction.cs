using System.Collections;

public class DiscardAction : IActionable {

	private Card card;

	public DiscardAction(Card card) {
		this.card = card;
	}

	public IEnumerator Perform(Player player) {
		yield return GameManager.Instance.discardPile.Push(card);
	}

	public void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(
			new GainCoinsResolver(player, () => {
				return GameOptions.DiscardCoinAmount;
			}),
			Priority.GainCoins
		);
	}

}
