using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Player : MonoBehaviour {

	[System.Serializable]
	public class DeckEntry {

		public DeckType deckType;
		public Deck deck;

	}

	public DeckEntry[] decks;
	public Hand hand;
	public CardSlot preparedCardSlot;
	public BuildDisplay buildDisplay;
	public MilitaryTokenDisplay militaryTokenDisplay;
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
	public HashSet<string> BuiltCards { get; private set; }
	public Dictionary<CardType, List<Card>> BuiltCardsByType { get; private set; }
	public string Nickname { get; set; }

	public List<ResourceOptions> resources;
	public List<ResourceOptions> ProducedResources {
		get {
			List<ResourceOptions> producedResources = new List<ResourceOptions>();
			foreach (ResourceOptions resource in resources) {
				if (resource.IsProduced) {
					producedResources.Add(resource);
				}
			}

			return producedResources;
		}
	}
	public Dictionary<KeyValuePair<Direction, Resource>, int> ResourceBuyCosts { get; private set; }
	public int ShieldCount { get; private set; }
	public bool IsPeaceful { get; set; }
	private List<ScienceOptions> sciences;

	void Awake() {
		Decks = new Dictionary<DeckType, Deck>();
		foreach (DeckEntry deckEntry in decks) {
			Decks.Add(deckEntry.deckType, deckEntry.deck);
		}
		Decks.Add(DeckType.Discard, GameManager.Instance.discardPile);
		Neighbours = new Dictionary<Direction, Player>();
		BuiltCards = new HashSet<string>();
		BuiltCardsByType = new Dictionary<CardType, List<Card>>();
		foreach (CardType cardType in Enum.GetValues(typeof(CardType))) {
			BuiltCardsByType[cardType] = new List<Card>();
		}
		ResourceBuyCosts = new Dictionary<KeyValuePair<Direction, Resource>, int>();
		foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
			foreach (Resource resource in Enum.GetValues(typeof(Resource))) {
				ResourceBuyCosts[new KeyValuePair<Direction, Resource>(direction, resource)] =
					GameOptions.InitialBuyCost;
			}
		}
		resources = new List<ResourceOptions>();
		sciences = new List<ScienceOptions>();
	}

	public abstract void DecideBuild(Card card, Payment payment);

	public abstract void DecideBury(Card card, int wonderStage, Payment payment);

	public abstract void DecideDiscard(Card card);

	public IEnumerator SetWonder(Wonder wonder) {
		yield return wonderSlot.Push(wonder);
		bank = wonder.bank;
		preparedCardSlot = wonder.preparedCardSlot;
		militaryTokenDisplay = wonder.militaryTokenDisplay;
		foreach (WonderStage wonderStage in wonder.wonderStages) {
			wonderStage.buryDropArea.onDropEvent.AddListener(DecideBury);
		}

		foreach (OnBuildEffect onBuildEffect in wonder.onBuildEffects) {
			onBuildEffect.Effect(this);
		}
	}

	public IEnumerator PrepareBuild(int positionInHand, Payment payment) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = new BuildAction(card, payment);
	}

	public IEnumerator PrepareBury(int positionInHand, int wonderStage, Payment payment) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = new BuryAction(card, wonderStage, payment);
	}

	public IEnumerator PrepareDiscard(int positionInHand) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = new DiscardAction(card);
	}

	public IEnumerator PerformAction() {
		while (Action == null) {
			// Wait until action is set
			yield return null;
		}

		Card card = preparedCardSlot.Pop();
		yield return Action.Perform(this);
	}
	
	public void EffectAction() {
		Action.Effect(this);
		Action = null;
	}

	public IEnumerator GainCoins(int amount) {
		yield return bank.PushMany(GameManager.Instance.bank.PopMany(amount));
	}

	public IEnumerator LoseCoins(int amount) {
		yield return GameManager.Instance.bank.PushMany(bank.PopMany(amount));
	}

	public IEnumerator GainPoints(PointType pointType, int amount) {
		yield return UIManager.Instance.scoreboard.AddPoints(this, pointType, amount);
	}

	public IEnumerator GainMilitaryToken(MilitaryToken militaryToken) {
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(this, PointType.Military, () => {
				return militaryToken.points;
			}),
			Priority.GainPoints
		);
		yield return militaryTokenDisplay.Push(militaryToken);
	}

	public void AddResource(ResourceOptions resourceOptions) {
		resources.Add(resourceOptions);
	}

	/// <summary>
	/// Adds a science entry for this player and returns the points gained by playing  this science.
	/// </summary>
	public int AddScience(ScienceOptions scienceOptions) {
		int pointsBefore = CalculateSciencePoints();
		sciences.Add(scienceOptions);
		int pointsAfter = CalculateSciencePoints();

		return pointsAfter - pointsBefore;
	}

	public void AddShield(int shieldsToAdd) {
		ShieldCount += shieldsToAdd;
	}

	public void EnableDropAreas(Card card) {
		EnableBuildAreas(card);
		EnableBuryAreas(card);
		EnableDiscardArea();
	}

	public void DisableDropAreas() {
		buildDropArea.IsPlayable = false;
		foreach (WonderStage wonderStage in Wonder.wonderStages) {
			wonderStage.IsPlayable = false;
		}
		discardDropArea.IsPlayable = false;
	}

	public int CountResource(Resource countedResource) {
		int count = 0;
		foreach (ResourceOptions resourceOptions in resources) {
			if (!resourceOptions.IsProduced) {
				// Only count produced resources
				continue;
			}

			foreach (Resource resource in resourceOptions.Resources) {
				if (resource == countedResource) {
					count++;
				}
			}
		}

		return count;
	}

	private void EnableBuildAreas(Card card) {
		if (BuiltCards.Contains(card.cardName)) {
			// Can't build duplicate cards
			return;
		}

		List<Payment> possiblePayments = new List<Payment>(
			PaymentResolver.Instance.Resolve(this, card)
		);
		if (possiblePayments.Count == 0) {
			// No payment combinations possible for this card
			return;
		}

		Payment cheapestPayment = possiblePayments[0];
		foreach (Payment payment in possiblePayments) {
			if (payment < cheapestPayment) {
				cheapestPayment = payment;
			}
		}

		if (cheapestPayment.TotalAmount <= bank.Count) {
			buildDropArea.payment = cheapestPayment;
			buildDropArea.IsPlayable = true;
		}
	}

	private void EnableBuryAreas(Card card) {
		WonderStage[] buildableStages = Wonder.GetBuildableStages();
		foreach (WonderStage stage in buildableStages) {
			List<Payment> possiblePayments = new List<Payment>(
				PaymentResolver.Instance.Resolve(this, stage, card)
			);
			if (possiblePayments.Count == 0) {
				// No payment combinations possible for this wonder stage
				continue;
			}

			Payment cheapestPayment = possiblePayments[0];
			foreach (Payment payment in possiblePayments) {
				if (payment < cheapestPayment) {
					cheapestPayment = payment;
				}
			}

			if (cheapestPayment.TotalAmount <= bank.Count) {
				stage.buryDropArea.payment = cheapestPayment;
				stage.buryDropArea.IsPlayable = true;
			}
		}
	}

	private void EnableDiscardArea() {
		discardDropArea.IsPlayable = true;
	}

	private int CalculateSciencePoints(int pos = 0, int[] counts = null) {
		if (pos == sciences.Count) {
			return 0;
		}

		if (counts == null) {
			counts = new int[Enum.GetNames(typeof(Science)).Length];
		}

		int maxPoints = 0;
		foreach (Science science in sciences[pos].Sciences) {
			int prevSymbolCount = counts[(int)science];
			int prevSetCount = counts.Min();

			counts[(int)science]++;

			int symbolCount = counts[(int)science];
			int setCount = counts.Min();

			maxPoints = Mathf.Max(
				maxPoints,
				CalculateSciencePoints(pos + 1, counts) +
					symbolCount * symbolCount - prevSymbolCount * prevSymbolCount +
					GameOptions.PointsPerScienceSet * (setCount - prevSetCount)
			);

			counts[(int)science]--;
		}

		return maxPoints;
	}

}
