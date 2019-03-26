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
		void Effect(Player player);

	}

	public class BuildAction : IActionable {

		private Card card;

		public BuildAction(Card card) {
			this.card = card;
		}

		public IEnumerator Action(Player player) {
			yield return card.Flip();
			yield return player.buildDisplay.Push(card);
			player.BuiltCardsByType[card.cardType].Add(card);
		}

		public void Effect(Player player) {
			foreach (OnBuildEffect onBuildEffect in card.onBuildEffects) {
				onBuildEffect.Effect(player);
			}
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
			yield return player.Wonder.wonderStages[wonderStage].buildCardSlot.Push(card);
		}

		public void Effect(Player player) {
			foreach (OnBuildEffect onBuildEffect in player.Wonder.wonderStages[wonderStage].onBuildEffects) {
				onBuildEffect.Effect(player);
			}
		}

	}

	public class DiscardAction : IActionable {

		private Card card;

		public DiscardAction(Card card) {
			this.card = card;
		}

		public IEnumerator Action(Player player) {
			yield return GameManager.Instance.discardPile.Push(card);
		}

		public void Effect(Player player) {
			GameManager.Instance.EnqueueResolver(
				new GainCoinsResolver(player, GameOptions.DiscardCoinAmount), 5
			);
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
	public abstract bool IsPlayable {
		set;
	}
	public IActionable Action { get; protected set; }
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
	}
	
	public void EffectAction() {
		Action.Effect(this);
	}

	public IEnumerator GainCoin(int amount) {
		yield return bank.PushMany(GameManager.Instance.bank.PopMany(amount));
	}

}
