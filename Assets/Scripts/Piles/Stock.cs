using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of cards to be dealt.
public class Stock : Pile<Card>, ILoadable, IShuffleable, IDealable {

	private const float DropSpacing = 0.25f;

	public Card[] initialCardPrefabs;
	public Stock[] shuffleStocks;
	public Facing facing;
	public Stack<Card> Cards { get; protected set; }
	public override int Count {
		get {
			return Cards.Count;
		}
	}

	void Awake() {
		Cards = new Stack<Card>();
	}

	public override IEnumerator Push(Card card) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Cards.Count + 1);
		Vector3 dropEulerAngles = transform.rotation.eulerAngles;
		if (facing == Facing.Up) {
			dropEulerAngles.z = 0.0f;
		} else {
			dropEulerAngles.z = 180.0f;
		}
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);
		yield return card.MoveTowards(dropPosition, dropRotation, 100, 1080);

		Cards.Push(card);
		card.transform.parent = transform;
	}

	public override Card Pop() {
		if (Cards.Count == 0) {
			return null;
		}

		return Cards.Pop();
	}

	public virtual IEnumerator Load() {
		for (int i = initialCardPrefabs.Length - 1; i >= 0; i--) {
			Card card = Instantiate(initialCardPrefabs[i], transform.position, transform.rotation);
			yield return Push(card);
		}
	}

	public IEnumerator Shuffle(int numIterations) {
		if (Cards.Count < 2) {
			// Less than 2 cards, no point in shuffling
			yield break;
		}

		for (int i = 0; i < numIterations; i++) {
			// Move each card to a random shuffle stock
			while (Cards.Count != 0) {
				yield return shuffleStocks[Random.Range(0, shuffleStocks.Length)].Push(Pop());
			}

			// Merge all shuffle stocks
			foreach (Stock shuffleStock in shuffleStocks) {
				yield return PushMany(shuffleStock.PopMany(shuffleStock.Count));
			}
		}
	}

	public IEnumerator Deal(DeckType deckType) {
		int playerIndex = 0;
		while (Cards.Count != 0) {
			yield return GameManager.Instance.players[playerIndex].Decks[deckType].Push(Pop());
			playerIndex = (playerIndex + 1) % GameManager.Instance.players.Length;
		}
	}

}
