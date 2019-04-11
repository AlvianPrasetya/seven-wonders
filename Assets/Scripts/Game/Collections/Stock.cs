using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of cards to be dealt.
public class Stock : CardPile, ILoadable, IShuffleable, IDealable {

	[System.Serializable]
	public class CardEntry {

		public Card cardPrefab;
		public int minPlayers;

	}

	public CardEntry[] initialCardEntries;
	public CardPile[] rifflePiles;

	public virtual IEnumerator Load() {
		foreach (CardEntry cardEntry in initialCardEntries) {
			if (cardEntry.minPlayers > GameManager.Instance.Players.Count) {
				// Not enough players to put this card into play
				continue;
			}

			Card card = Instantiate(cardEntry.cardPrefab, transform.position, transform.rotation);
			yield return Push(card);
		}
	}

	public IEnumerator Shuffle(int numIterations, int randomSeed) {
		if (Elements.Count < 2) {
			// Less than 2 cards, no point in shuffling
			yield break;
		}

		System.Random random = new System.Random(randomSeed);
		int numElements = Elements.Count;
		Queue<Coroutine> loadRifflePiles = new Queue<Coroutine>();
		for (int i = 0; i < numIterations; i++) {
			// Move half of the stock to the left riffle pile
			while (Elements.Count != numElements / 2) {
				loadRifflePiles.Enqueue(StartCoroutine(rifflePiles[0].Push(Pop())));
			}
			// Move the other half to the right riffle pile
			while (Elements.Count != 0) {
				loadRifflePiles.Enqueue(StartCoroutine(rifflePiles[1].Push(Pop())));
			}
			while (loadRifflePiles.Count != 0) {
				yield return loadRifflePiles.Dequeue();
			}

			while (rifflePiles[0].Count != 0 && rifflePiles[1].Count != 0) {
				yield return Push(rifflePiles[random.Next(0, rifflePiles.Length)].Pop());
			}
			while (rifflePiles[0].Count != 0) {
				yield return Push(rifflePiles[0].Pop());
			}
			while (rifflePiles[1].Count != 0) {
				yield return Push(rifflePiles[1].Pop());
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
