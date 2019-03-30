using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of cards to be dealt.
public class Stock : CardPile, ILoadable, IShuffleable, IDealable {

	public Card[] initialCardPrefabs;
	public CardPile[] shuffleCardPiles;

	public virtual IEnumerator Load() {
		foreach (Card initialCardPrefab in initialCardPrefabs) {
			if (initialCardPrefab is StructureCard &&
				((StructureCard)initialCardPrefab).minPlayers > GameManager.Instance.Players.Count
			) {
				continue;
			}

			Card card = Instantiate(initialCardPrefab, transform.position, transform.rotation);
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
			foreach (CardPile shuffleCardPile in shuffleCardPiles) {
				yield return PushMany(shuffleCardPile.PopMany(shuffleCardPile.Count));
			}
		}
	}

	public IEnumerator Deal(DeckType deckType) {
		int playerIndex = 0;
		Queue<Coroutine> dealCoins = new Queue<Coroutine>();
		while (Elements.Count != 0) {
			dealCoins.Enqueue(StartCoroutine(
				GameManager.Instance.Players[playerIndex].Decks[deckType].Push(Pop())
			));
			playerIndex = (playerIndex + 1) % GameManager.Instance.Players.Count;

			if (dealCoins.Count == GameManager.Instance.Players.Count) {
				// Deal to all players at a time
				while (dealCoins.Count != 0) {
					yield return dealCoins.Dequeue();
				}
			}
		}
		while (dealCoins.Count != 0) {
			yield return dealCoins.Dequeue();
		}
	}

}
