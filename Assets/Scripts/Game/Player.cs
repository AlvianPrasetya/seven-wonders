using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour {

	[System.Serializable]
	public class DeckEntry {

		public DeckType deckType;
		public Deck deck;

	}

	public delegate IEnumerator TurnAction(
		Card card, params object[] args
	);

	public DeckEntry[] decks;
	public Hand hand;
	public CardSlot preparedCardSlot;
	public BuildDisplay buildDisplay;
	public BuildDropArea buildDropArea;
	public DiscardDropArea discardDropArea;
	public WonderSlot wonderSlot;
	public Bank bank;

	public Dictionary<DeckType, Deck> Decks { get; private set; }
	public Dictionary<Direction, Player> Neighbours { get; private set; }
	public Wonder Wonder {
		get {
			return wonderSlot.Element;
		}
	}
	public bool IsActive {
		set {
			buildDropArea.IsActive = value;
			discardDropArea.IsActive = value;
			Wonder.IsActive = value;
		}
	}
	public TurnAction Action { get; private set; }
	public Dictionary<CardType, List<Card>> BuiltCardsByType { get; private set; }

	void Awake() {
		Decks = new Dictionary<DeckType, Deck>();
		foreach (DeckEntry deckEntry in decks) {
			Decks.Add(deckEntry.deckType, deckEntry.deck);
		}
		Neighbours = new Dictionary<Direction, Player>();
		BuiltCardsByType = new Dictionary<CardType, List<Card>>();
		foreach (CardType cardType in Enum.GetValues(typeof(CardType))) {
			BuiltCardsByType[cardType] = new List<Card>();
		}
	}

	public abstract void DecideBuild(Card card);

	public abstract void DecideBury(Card card, int wonderStage);

	public abstract void DecideDiscard(Card card);

	public IEnumerator PrepareBuild(int positionInHand) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = Build;
	}

	public IEnumerator PrepareBury(int positionInHand, int wonderStage) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = Bury;
	}

	public IEnumerator PrepareDiscard(int positionInHand) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = Discard;
	}

	public IEnumerator PerformAction() {
		Card card = preparedCardSlot.Pop();
		yield return Action(card);
		Action = null;
	}

	public IEnumerator Build(Card card, object unused) {
		yield return card.Flip();
		yield return buildDisplay.Push(card);

		BuiltCardsByType[card.cardType].Add(card);
		foreach (OnBuildEffect onBuildEffect in card.onBuildEffects) {
			onBuildEffect.Effect(this);
		}
	}

	public IEnumerator Bury(Card card, object wonderStage) {
		yield return Wonder.wonderStages[(int)wonderStage].buildCardSlot.Push(card);
		foreach (OnBuildEffect onBuildEffect in Wonder.wonderStages[(int)wonderStage].onBuildEffects) {
			onBuildEffect.Effect(this);
		}
	}

	public IEnumerator Discard(Card card, object unused) {
		yield return GameManager.Instance.discardPile.Push(card);
		GameManager.Instance.EnqueueResolver(
			new GainCoinsResolver(this, GameOptions.DiscardCoinAmount), 5
		);
	}

	public IEnumerator GainCoin(int amount) {
		yield return bank.PushMany(GameManager.Instance.bank.PopMany(amount));
	}

}
