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

	public interface IActionable {

		IEnumerator Action(Player player);

	}

	public class BuildAction : IActionable {

		private Card card;

		public BuildAction(Card card) {
			this.card = card;
		}

		public IEnumerator Action(Player player) {
			yield return player.Build(card);
		}

	}

	public class BuryAction : IActionable {

		private Card card;
		private int wonderStage;

		public BuryAction(Card card, int wonderStage) {
			this.card = card;
			this.wonderStage = wonderStage;
		}

		public IEnumerator Action(Player player) {
			yield return player.Bury(card, wonderStage);
		}

	}

	public class DiscardAction : IActionable {

		private Card card;

		public DiscardAction(Card card) {
			this.card = card;
		}

		public IEnumerator Action(Player player) {
			yield return player.Discard(card);
		}

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
	public IActionable Action { get; private set; }
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
		Action = new BuildAction(card);
	}

	public IEnumerator PrepareBury(int positionInHand, int wonderStage) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = new BuryAction(card, wonderStage);
	}

	public IEnumerator PrepareDiscard(int positionInHand) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = new DiscardAction(card);
	}

	public IEnumerator PerformAction() {
		Card card = preparedCardSlot.Pop();
		yield return Action.Action(this);
		Action = null;
	}

	public IEnumerator Build(Card card) {
		yield return card.Flip();
		yield return buildDisplay.Push(card);

		BuiltCardsByType[card.cardType].Add(card);
		foreach (OnBuildEffect onBuildEffect in card.onBuildEffects) {
			onBuildEffect.Effect(this);
		}
	}

	public IEnumerator Bury(Card card, int wonderStage) {
		yield return Wonder.wonderStages[wonderStage].buildCardSlot.Push(card);
		foreach (OnBuildEffect onBuildEffect in Wonder.wonderStages[wonderStage].onBuildEffects) {
			onBuildEffect.Effect(this);
		}
	}

	public IEnumerator Discard(Card card) {
		yield return GameManager.Instance.discardPile.Push(card);
		GameManager.Instance.EnqueueResolver(
			new GainCoinsResolver(this, GameOptions.DiscardCoinAmount), 5
		);
	}

	public IEnumerator GainCoin(int amount) {
		yield return bank.PushMany(GameManager.Instance.bank.PopMany(amount));
	}

}
