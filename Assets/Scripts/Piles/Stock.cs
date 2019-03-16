using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of loadable, shuffleable and dealable cards.
public class Stock : Pile<Card>, ILoadable, IShuffleable, IDealable {

	private const float DropSpacing = 0.25f;

	public Card[] initialCardPrefab;
	public Stock[] shuffleStocks;
	public LinkedList<Card> Cards { get; private set; }
	public override int Count {
		get {
			return Cards.Count;
		}
	}

	void Awake() {
		Cards = new LinkedList<Card>();
	}

	void Start() {
		StartCoroutine(Load());
	}

	public override IEnumerator Push(Card card) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Cards.Count + 1);
		yield return card.MoveTowards(dropPosition, transform.rotation, 100);

		Cards.AddLast(card);
	}

	public override Card Pop() {
		if (Cards.Count == 0) {
			return null;
		}

		Card topCard = Cards.Last.Value;
		Cards.RemoveLast();
		return topCard;
	}

	public IEnumerator Load() {
		foreach (Card cardPrefab in initialCardPrefab) {
			Card card = Instantiate(cardPrefab, transform.position, transform.rotation);
			yield return Push(card);
		}

		yield return new WaitForSeconds(3.0f);
		yield return Shuffle(10);
	}

	public IEnumerator Shuffle(int numIterations) {
		if (Cards.Count < 2) {
			// Less than 2 cards, no point in shuffling
			yield break;
		}

		for (int i = 0; i < numIterations; i++) {
			// Move each card to a random shuffle stock
			while (Cards.Count != 0) {
				yield return shuffleStocks[Random.Range(0, shuffleStocks.Length)].Push(PopBottom());
			}

			// Merge all shuffle stocks
			foreach (Stock shuffleStock in shuffleStocks) {
				yield return PushMany(shuffleStock.PopBottomMany(shuffleStock.Count));
			}
		}
	}

	public IEnumerator Deal() {
		yield return null;
	}

	protected IEnumerator PushMany(Card[] cards) {
		foreach (Card card in cards) {
			yield return Push(card);
		}
	}

	protected Card PopBottom() {
		if (Cards.Count == 0) {
			return null;
		}

		Card bottomCard = Cards.First.Value;
		Cards.RemoveFirst();
		return bottomCard;
	}

	protected Card[] PopBottomMany(int count) {
		List<Card> poppedCards = new List<Card>();
		while (poppedCards.Count != count && Cards.Count != 0) {
			poppedCards.Add(PopBottom());
		}

		return poppedCards.ToArray();
	}

}
