using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of loadable, shuffleable and dealable cards.
public class Stock : Pile<Card>, ILoadable, IShuffleable, IDealable {

	private const float DropSpacing = 0.25f;

	public Card[] initialCardPrefabs;
	public Stock[] shuffleStocks;

	void Awake() {
		Elements = new LinkedList<Card>();
	}

	public override IEnumerator Push(Card card) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Elements.Count + 1);
		yield return card.MoveTowards(dropPosition, transform.rotation, 100, 360);

		Elements.AddLast(card);
	}

	public override Card Pop() {
		if (Elements.Count == 0) {
			return null;
		}

		Card topCard = Elements.Last.Value;
		Elements.RemoveLast();
		return topCard;
	}

	public virtual IEnumerator Load() {
		for (int i = initialCardPrefabs.Length - 1; i >= 0; i--) {
			Card card = Instantiate(initialCardPrefabs[i], transform.position, transform.rotation);
			yield return Push(card);
		}
	}

	public IEnumerator Shuffle(int numIterations) {
		if (Elements.Count < 2) {
			// Less than 2 cards, no point in shuffling
			yield break;
		}

		for (int i = 0; i < numIterations; i++) {
			// Move each card to a random shuffle stock
			while (Elements.Count != 0) {
				yield return shuffleStocks[Random.Range(0, shuffleStocks.Length)].Push(Pop());
			}

			// Merge all shuffle stocks
			foreach (Stock shuffleStock in shuffleStocks) {
				yield return PushMany(shuffleStock.PopMany(shuffleStock.Count));
			}
		}
	}

	public IEnumerator Deal(int count) {
		yield return null;
	}

	public Card PopBottom() {
		if (Elements.Count == 0) {
			return null;
		}

		Card bottomCard = Elements.First.Value;
		Elements.RemoveFirst();
		return bottomCard;
	}

	public Card[] PopBottomMany(int count) {
		List<Card> poppedCards = new List<Card>();
		while (poppedCards.Count != count && Elements.Count != 0) {
			poppedCards.Add(PopBottom());
		}

		return poppedCards.ToArray();
	}

	public Card PeekBottom() {
		if (Elements.Count == 0) {
			return null;
		}

		return Elements.First.Value;
	}

}
