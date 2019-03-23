using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class DeckEntry {

		public DeckType deckType;
		public Deck deck;

	}

	public delegate IEnumerator TurnAction(Card card);

	public DeckEntry[] decks;
	public Hand hand;
	public CardSlot preparedCardSlot;
	public BuildDisplay buildDisplay;
	public BuildDropArea buildDropArea;
	public DiscardDropArea discardDropArea;
	public Wonder wonder;

	public Dictionary<DeckType, Deck> Decks { get; private set; }
	public Dictionary<Direction, Player> Neighbours { get; private set; }
	public bool IsActive {
		set {
			buildDropArea.IsActive = value;
			discardDropArea.IsActive = value;
			wonder.IsPlayable = value;
		}
	}
	public TurnAction Action { get; private set; }

	void Awake() {
		Decks = new Dictionary<DeckType, Deck>();
		foreach (DeckEntry deckEntry in decks) {
			Decks.Add(deckEntry.deckType, deckEntry.deck);
		}
		Neighbours = new Dictionary<Direction, Player>();
	}

	public void PlayBuild(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBuild(positionInHand);
	}

	public void PlayBury(Card card, int wonderStage) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBury(positionInHand, wonderStage);
	}

	public void PlayDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideDiscard(positionInHand);
	}

	public IEnumerator PrepareBuild(int positionInHand) {
		yield return preparedCardSlot.Push(hand.PopAt(positionInHand));
		Action = Build;
	}

	public IEnumerator PrepareBury(int positionInHand, int wonderStage) {
		yield return preparedCardSlot.Push(hand.PopAt(positionInHand));
		Action = wonder.wonderStages[wonderStage].Build;
	}

	public IEnumerator PrepareDiscard(int positionInHand) {
		yield return preparedCardSlot.Push(hand.PopAt(positionInHand));
		Action = Discard;
	}

	public IEnumerator PerformAction() {
		yield return Action(preparedCardSlot.Pop());
		Action = null;
	}

	public IEnumerator Build(Card card) {
		yield return card.Flip();
		yield return new WaitForSeconds(1);
		yield return buildDisplay.Push(card);
	}

	public IEnumerator Discard(Card card) {
		yield return GameManager.Instance.discardPile.Push(card);
	}

}
