using System.Collections;

public class DiscardPile : Deck {

	public override IEnumerator Push(Card card) {
		yield return base.Push(card);

		// Set discarded cards as virtually free
		card.coinCost = 0;
		card.resourceCost = new Resource[0];
	}

}
