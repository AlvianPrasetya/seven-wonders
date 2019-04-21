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
	public DraftDropArea draftDropArea;
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
	public bool IsPlayable {
		set {
			if (value) {
				hand.IsPlayable = true;
			} else {
				hand.IsPlayable = false;
			}
		}
	}
	public IActionable Action { get; protected set; }
	public HashSet<string> BuiltCards { get; private set; }
	public Dictionary<CardType, List<Card>> BuiltCardsByType { get; private set; }
	public string Nickname { get; set; }

	public List<Resource> Resources { get; private set; }
	public List<ConditionalResource> ConditionalResources { get; private set; }
	public List<Resource> ProducedResources {
		get {
			List<Resource> producedResources = new List<Resource>();
			foreach (Resource resource in Resources) {
				if (resource.IsProduced) {
					producedResources.Add(resource);
				}
			}

			return producedResources;
		}
	}
	public Dictionary<KeyValuePair<Direction, ResourceType>, int> ResourceBuyCosts { get; private set; }
	public int ShieldCount { get; private set; }
	public bool IsPeaceful { get; set; }
	public List<Science> Sciences { get; private set; }
	public List<Science> ProducedSciences {
		get {
			List<Science> producedSciences = new List<Science>();
			foreach (Science science in Sciences) {
				if (science.IsProduced) {
					producedSciences.Add(science);
				}
			}

			return producedSciences;
		}
	}

	// Resolvers
	public PaymentResolver PaymentResolver {
		get; private set;
	}
	public SciencePointsResolver SciencePointsResolver {
		get; private set;
	}

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
		ResourceBuyCosts = new Dictionary<KeyValuePair<Direction, ResourceType>, int>();
		foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
			foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType))) {
				ResourceBuyCosts[new KeyValuePair<Direction, ResourceType>(direction, resource)] =
					GameOptions.InitialBuyCost;
			}
		}
		Resources = new List<Resource>();
		ConditionalResources = new List<ConditionalResource>();
		Sciences = new List<Science>();

		PaymentResolver = new PaymentResolver(this);
		SciencePointsResolver = new SciencePointsResolver(this);
	}

	public abstract void DecideDraft(Card card);

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

	public IEnumerator PrepareDraft(int positionInHand) {
		Card card = hand.PopAt(positionInHand);
		yield return preparedCardSlot.Push(card);
		Action = new DraftAction(card);
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

	public IEnumerator Pay(Payment payment) {
		// Pay bank and neighbours
		yield return GameManager.Instance.bank.PushMany(
			bank.PopMany(payment.PayBankAmount)
		);
		yield return Neighbours[Direction.West].bank.PushMany(
			bank.PopMany(payment.PayWestAmount)
		);
		yield return Neighbours[Direction.East].bank.PushMany(
			bank.PopMany(payment.PayEastAmount)
		);

		/*if (this == GameManager.Instance.Player) {
			UIManager.Instance.chat.AddMessage(
				string.Format(
					"You paid {0} coins to the bank, {1} coins to <b>{2}</b>, and {3} coins to <b>{4}</b>",
					payment.PayBankAmount,
					payment.PayWestAmount,
					Neighbours[Direction.West].Nickname,
					payment.PayEastAmount,
					Neighbours[Direction.East].Nickname
				)
			);
		}*/
	}

	public IEnumerator GainCoins(int amount) {
		yield return bank.PushMany(GameManager.Instance.bank.PopMany(amount));

		if (this == GameManager.Instance.Player) {
			UIManager.Instance.chat.AddMessage(
				string.Format("You gained {0} coins", amount
			));
		}
	}

	public IEnumerator LoseCoins(int amount) {
		yield return GameManager.Instance.bank.PushMany(bank.PopMany(amount));
		
		if (this == GameManager.Instance.Player) {
			UIManager.Instance.chat.AddMessage(
				string.Format("You lost {0} coins", amount)
			);
		}
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

	public IEnumerator RemoveMilitaryToken(MilitaryToken militaryToken) {
		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(this, PointType.Military, () => {
				return -1 * militaryToken.points;
			}),
			Priority.GainPoints
		);
		yield return militaryTokenDisplay.Remove(militaryToken);
	}

	public void AddResource(Resource resource) {
		Resources.Add(resource);
	}

	public void AddConditionalResource(ConditionalResource conditionalResource) {
		ConditionalResources.Add(conditionalResource);
	}

	/// <summary>
	/// Adds a science entry for this player and returns the points gained by playing  this science.
	/// </summary>
	public int AddScience(Science science) {
		int pointsBefore = SciencePointsResolver.ResolvePoints();
		Sciences.Add(science);
		int pointsAfter = SciencePointsResolver.ResolvePoints();

		return pointsAfter - pointsBefore;
	}

	public int AddPointsPerScienceSet(int extraPointsPerScienceSet) {
		int pointsBefore = SciencePointsResolver.ResolvePoints();
		SciencePointsResolver.AddPointsPerScienceSet(extraPointsPerScienceSet);
		int pointsAfter = SciencePointsResolver.ResolvePoints();

		return pointsAfter - pointsBefore;
	}

	public void AddShield(int shieldsToAdd) {
		ShieldCount += shieldsToAdd;
	}

	public void EnableDraftArea() {
		draftDropArea.IsPlayable = true;
	}

	public void EnableBuildArea(Card card) {
		if (BuiltCards.Contains(card.cardName)) {
			// Can't build duplicate cards
			return;
		}

		List<Payment> possiblePayments = new List<Payment>(
			PaymentResolver.Resolve(this, card)
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

	public void EnableBuryAreas(Card card) {
		WonderStage[] buildableStages = Wonder.GetBuildableStages();
		foreach (WonderStage stage in buildableStages) {
			List<Payment> possiblePayments = new List<Payment>(
				PaymentResolver.Resolve(this, stage, card)
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

	public void EnableDiscardArea() {
		discardDropArea.IsPlayable = true;
	}

	public void DisableDraftArea() {
		draftDropArea.IsPlayable = false;
	}

	public void DisableBuildArea() {
		buildDropArea.IsPlayable = false;
	}

	public void DisableBuryAreas() {
		foreach (WonderStage wonderStage in Wonder.wonderStages) {
			wonderStage.IsPlayable = false;
		}
	}

	public void DisableDiscardArea() {
		discardDropArea.IsPlayable = false;
	}

	public int CountResource(ResourceType countedResourceType) {
		int count = 0;
		foreach (Resource resource in Resources) {
			if (!resource.IsProduced) {
				// Only count produced resources
				continue;
			}

			foreach (ResourceType resourceType in resource.ResourceTypes) {
				if (resourceType == countedResourceType) {
					count++;
				}
			}
		}

		return count;
	}

}
