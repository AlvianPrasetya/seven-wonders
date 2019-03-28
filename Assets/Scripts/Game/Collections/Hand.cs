using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour, IPushable<Card>, IPoppable<Card> {

	public CardPile[] cardPiles;
	public Facing Facing {
		set {
			foreach (CardPile displayPile in cardPiles) {
				displayPile.facing = value;
			}
		}
	}

	public int Count {
		get {
			int count = 0;
			foreach (CardPile displayPile in cardPiles) {
				count += displayPile.Count;
			}
			
			return count;
		}
	}

	public IEnumerator Push(Card card) {
		yield return Push(card, Direction.West);
	}

	public IEnumerator Push(Card card, Direction pushTowards) {
		CardPile targetPile;
		switch (pushTowards) {
			case Direction.West:
				// Try to find westernmost empty display pile, put on easternmost pile if not found
				targetPile = cardPiles[cardPiles.Length - 1];
				for (int i = 0; i < cardPiles.Length; i++) {
					if (cardPiles[i].Count == 0) {
						targetPile = cardPiles[i];
						break;
					}
				}
				break;
			case Direction.East:
				// Try to find easternmost empty display pile, put on westernmost pile if not found
				targetPile = cardPiles[0];
				for (int i = cardPiles.Length - 1; i >= 0; i--) {
					if (cardPiles[i].Count == 0) {
						targetPile = cardPiles[i];
						break;
					}
				}
				break;
			default:
				yield break;
		}

		yield return targetPile.Push(card);
	}

	public IEnumerator PushMany(Card[] cards) {
		yield return PushMany(cards, Direction.West);
	}

	public IEnumerator PushMany(Card[] cards, Direction pushTowards) {
		foreach (Card card in cards) {
			yield return Push(card, pushTowards);
		}
	}

	public Card Pop() {
		return Pop(Direction.West);
	}

	public Card Pop(Direction popFrom) {
		switch (popFrom) {
			case Direction.West:
				for (int i = 0; i < cardPiles.Length; i++) {
					if (cardPiles[i].Count != 0) {
						return cardPiles[i].Pop();
					}
				}
				break;
			case Direction.East:
				for (int i = cardPiles.Length - 1; i >= 0; i--) {
					if (cardPiles[i].Count != 0) {
						return cardPiles[i].Pop();
					}
				}
				break;
		}

		return null;
	}

	public Card PopAt(int pileIndex) {
		if (pileIndex < 0 || pileIndex >= cardPiles.Length) {
			return null;
		}

		return cardPiles[pileIndex].Pop();
	}

	public Card[] PopMany(int count) {
		return PopMany(count, Direction.West);
	}

	public Card[] PopMany(int count, Direction popFrom) {
		List<Card> poppedCards = new List<Card>();
		while (poppedCards.Count != count && Count != 0) {
			poppedCards.Add(Pop(popFrom));
		}

		return poppedCards.ToArray();
	}

	public IEnumerator Unload(Deck targetDeck, Direction popDirection) {
		yield return targetDeck.PushMany(PopMany(Count, popDirection));
	}

	public int GetPosition(Card card) {
		for (int i = 0; i < cardPiles.Length; i++) {
			if (cardPiles[i].Peek() == card) {
				return i;
			}
		}

		throw new UnityException("The specified card could not be found");
	}

	/// <summary>
	/// Returns a random card in this hand.
	/// </summary>
	public Card GetRandom() {
		List<Card> cards = new List<Card>();
		foreach (CardPile displayPile in cardPiles) {
			if (displayPile.Count != 0) {
				cards.Add(displayPile.Peek());
			}
		}

		return cards[Random.Range(0, cards.Count)];
	}

}
