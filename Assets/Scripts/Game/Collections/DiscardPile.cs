using System.Collections;
using UnityEngine;

public class DiscardPile : Deck {

	public override IEnumerator Push(Card card) {
		if (card.GetType() == typeof(LeaderCard)) {
			// Is a leader card, destroy upon reaching discard pile
			Coroutine pushLeaderCard = StartCoroutine(base.Push(card));
			Pop();
			yield return pushLeaderCard;

			Destroy(card.gameObject);
			yield break;
		}

		yield return base.Push(card);

		// Set discarded cards as virtually free
		card.coinCost = 0;
		card.resourceCost = new Resource[0];
	}

}
