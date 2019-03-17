using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class DeckEntry {

		public DeckType deckType;
		public CardPile deck;

	}

	public DeckEntry[] decks;

	public Dictionary<DeckType, CardPile> Decks { get; private set; }

	void Awake() {
		Decks = new Dictionary<DeckType, CardPile>();
		foreach (DeckEntry deckEntry in decks) {
			Decks.Add(deckEntry.deckType, deckEntry.deck);
		}
	}

}
