using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class DeckEntry {

		public DeckType deckType;
		public Deck deck;

	}

	private delegate IEnumerator Action(Card card);

	public DeckEntry[] decks;
	public Hand hand;
	public CardSlot preparedCardSlot;
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
	private Action action;

	void Awake() {
		Decks = new Dictionary<DeckType, Deck>();
		foreach (DeckEntry deckEntry in decks) {
			Decks.Add(deckEntry.deckType, deckEntry.deck);
		}
	}

	public void PlayBuild(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.PlayBuild(positionInHand);
	}

	public void PlayBury(Card card, int wonderStage) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.PlayBury(positionInHand, wonderStage);
	}

	public void PlayDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.PlayDiscard(positionInHand);
	}

	public IEnumerator PrepareBuild(int positionInHand) {
		yield return preparedCardSlot.Push(hand.PopAt(positionInHand));
		action = Build;
	}

	public IEnumerator PrepareBury(int positionInHand, int wonderStage) {
		yield return preparedCardSlot.Push(hand.PopAt(positionInHand));
		action = wonder.wonderStages[wonderStage].Build;
	}

	public IEnumerator PrepareDiscard(int positionInHand) {
		yield return preparedCardSlot.Push(hand.PopAt(positionInHand));
		action = Discard;
	}

	public IEnumerator PerformAction() {
		yield return action(preparedCardSlot.Pop());
	}

	public IEnumerator Build(Card card) {
		yield return null;
	}

	public IEnumerator Discard(Card card) {
		yield return GameManager.Instance.discardPile.Push(card);
	}

}
