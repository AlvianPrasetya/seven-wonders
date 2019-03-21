using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of cards to be dealt.
public class Stock : CardPile, ILoadable, IShuffleable, IDealable {

	public Card[] initialCardPrefabs;
	public CardPile[] shuffleCardPiles;

	public virtual IEnumerator Load() {
		for (int i = 0; i < initialCardPrefabs.Length; i++) {
			Card card = Instantiate(initialCardPrefabs[i], transform.position, transform.rotation);
			yield return Push(card);
		}
	}

	public IEnumerator Shuffle(int numIterations, int randomSeed) {
		if (Elements.Count < 2) {
			// Less than 2 cards, no point in shuffling
			yield break;
		}

		System.Random random = new System.Random(randomSeed);

		for (int i = 0; i < numIterations; i++) {
			// Move each card to a random shuffle stock
			while (Elements.Count != 0) {
				yield return shuffleCardPiles[random.Next(0, shuffleCardPiles.Length)].Push(Pop());
			}

			// Merge all shuffle stocks
			foreach (CardPile shuffleCardPiles in shuffleCardPiles) {
				yield return PushMany(shuffleCardPiles.PopMany(shuffleCardPiles.Count));
			}
		}
	}

	public IEnumerator Deal(DeckType deckType) {
		int playerIndex = 0;
		while (Elements.Count != 0) {
			yield return GameManager.Instance.Players[playerIndex].Decks[deckType].Push(Pop());
			playerIndex = (playerIndex + 1) % GameManager.Instance.Players.Count;
		}
	}

}
