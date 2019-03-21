using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class DeckEntry {

		public DeckType deckType;
		public Deck deck;

	}

	public DeckEntry[] decks;
	public Hand hand;
	public Player westNeighbour;
	public Player eastNeighbour;
	public PlayArea buildPlayArea;
	public PlayArea discardPlayArea;
	public Wonder wonder;

	public Dictionary<DeckType, Deck> Decks { get; private set; }
	public bool IsPlayable {
		set {
			buildPlayArea.IsPlayable = value;
			discardPlayArea.IsPlayable = value;
			wonder.IsPlayable = value;
		}
	}

	void Awake() {
		Decks = new Dictionary<DeckType, Deck>();
		foreach (DeckEntry deckEntry in decks) {
			Decks.Add(deckEntry.deckType, deckEntry.deck);
		}
	}

	public void PlayBuild(Card card) {

	}

	public void PlayBury(Card card, int wonderStage) {

	}

	public void PlayDiscard(Card card) {

	}

}
