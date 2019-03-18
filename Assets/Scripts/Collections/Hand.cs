using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour, IPushable<Card>, IPoppable<Card>, IUnloadable<Card> {

	public CardPile[] displayPiles;

	public int Count {
		get {
			int count = 0;
			foreach (CardPile displayPile in displayPiles) {
				count += displayPile.Count;
			}
			
			return count;
		}
	}

	public IEnumerator Push(Card card) {
		// Try to find empty display pile first, put on last pile if not found
		CardPile targetPile = displayPiles[displayPiles.Length - 1];
		foreach (CardPile displayPile in displayPiles) {
			if (displayPile.Count == 0) {
				targetPile = displayPile;
				break;
			}
		}

		yield return targetPile.Push(card);
	}

	public IEnumerator PushMany(Card[] cards) {
		foreach (Card card in cards) {
			yield return Push(card);
		}
	}

	public Card Pop() {
		foreach (CardPile displayPile in displayPiles) {
			if (displayPile.Count != 0) {
				return displayPile.Pop();
			}
		}

		return null;
	}

	public Card[] PopMany(int count) {
		List<Card> poppedCards = new List<Card>();
		while (poppedCards.Count != count && Count != 0) {
			poppedCards.Add(Pop());
		}

		return poppedCards.ToArray();
	}

	public IEnumerator Unload(IPushable<Card> targetContainer) {
		yield return targetContainer.PushMany(PopMany(Count));
	}

}
