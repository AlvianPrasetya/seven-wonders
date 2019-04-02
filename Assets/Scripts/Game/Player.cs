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

		IEnumerator Perform(Player player);
		void Effect(Player player);

	}

	public class BuildAction : IActionable {

		private Card card;
		private Payment payment;

		public BuildAction(Card card, Payment payment) {
			this.card = card;
			this.payment = payment;
		}

		public IEnumerator Perform(Player player) {
			yield return card.Flip();

			int maxCardCount = 0;
			foreach (KeyValuePair<DisplayType, DisplayPile> kv in player.buildDisplay.DisplayPiles) {
				DisplayPile displayPile = kv.Value;
				if (displayPile.Count > maxCardCount) {
					maxCardCount = displayPile.Count;
				}
			}
			player.wonderSlot.spacing = new Vector3(0, 0.2f + (maxCardCount + 1) * 0.05f, 0);
			yield return player.wonderSlot.Push(player.wonderSlot.Pop());

			yield return player.buildDisplay.Push(card);
			player.BuiltCards.Add(card.cardName);
			player.BuiltCardsByType[card.cardType].Add(card);

			// Pay bank and neighbours
			yield return GameManager.Instance.bank.PushMany(
				player.bank.PopMany(payment.PayBankAmount)
			);
			yield return player.Neighbours[Direction.West].bank.PushMany(
				player.bank.PopMany(payment.PayWestAmount)
			);
			yield return player.Neighbours[Direction.East].bank.PushMany(
				player.bank.PopMany(payment.PayEastAmount)
			);
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
		private Payment payment;

		public BuryAction(Card card, int wonderStage, Payment payment) {
			this.card = card;
			this.wonderStage = wonderStage;
			this.payment = payment;
		}

		public IEnumerator Perform(Player player) {
			yield return player.Wonder.wonderStages[wonderStage].buildCardSlot.Push(card);

			// Pay bank and neighbours
			yield return GameManager.Instance.bank.PushMany(
				player.bank.PopMany(payment.PayBankAmount)
			);
			yield return player.Neighbours[Direction.West].bank.PushMany(
				player.bank.PopMany(payment.PayWestAmount)
			);
			yield return player.Neighbours[Direction.East].bank.PushMany(
				player.bank.PopMany(payment.PayEastAmount)
			);
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

		public IEnumerator Perform(Player player) {
			yield return GameManager.Instance.discardPile.Push(card);
		}

		public void Effect(Player player) {
			GameManager.Instance.EnqueueResolver(
				new GainCoinsResolver(player, GameOptions.DiscardCoinAmount), Priority.GainCoins
			);
		}

	}

	public delegate IEnumerator TurnAction(
		Card card, params object[] args
	);

	public struct PlayerResource {
		public Player player;
		public Resource resource;
		public PlayerResource(Player player, Resource resource) {
			this.player = player;
			this.resource = resource;
		}
	}

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
	public HashSet<string> BuiltCards { get; private set; }
	public Dictionary<CardType, List<Card>> BuiltCardsByType { get; private set; }
	public string Nickname { get; set; }
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
	public Dictionary<PlayerResource, int> ResourceBuyCosts { get; private set; }

	private List<ResourceOptions> resources;

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
		ResourceBuyCosts = new Dictionary<PlayerResource, int>();
		resources = new List<ResourceOptions>();
	}

	public abstract void DecideBuild(Card card, Payment payment);

	public abstract void DecideBury(Card card, int wonderStage, Payment payment);

	public abstract void DecideDiscard(Card card);

	public IEnumerator SetWonder(Wonder wonder) {
		yield return wonderSlot.Push(wonder);
		bank = wonder.bank;
		preparedCardSlot = wonder.preparedCardSlot;
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
		Card card = preparedCardSlot.Pop();
		yield return Action.Perform(this);
	}
	
	public void EffectAction() {
		Action.Effect(this);
	}

	public IEnumerator GainCoins(int amount) {
		yield return bank.PushMany(GameManager.Instance.bank.PopMany(amount));
	}

	public IEnumerator GainPoints(PointType pointType, int amount) {
		yield return UIManager.Instance.scoreboard.AddPoints(this, pointType, amount);
	}

	public void AddResource(ResourceOptions resourceOptions) {
		resources.Add(resourceOptions);
	}

	public void EnableDropAreas(Card card) {
		EnableBuildAreas(card);
		EnableBuryAreas();
		EnableDiscardArea();
	}

	public void DisableDropAreas() {
		buildDropArea.IsPlayable = false;
		foreach (WonderStage wonderStage in Wonder.wonderStages) {
			wonderStage.IsPlayable = false;
		}
		discardDropArea.IsPlayable = false;
	}

	// TODO: Refactor
	protected IEnumerable<Multiset<PlayerResource>> GetBoughtResourceSets(
		int pos, Multiset<Resource> resourceCost, Multiset<PlayerResource> toBuy
	) {
		if (resourceCost.Count == 0) {
			yield return toBuy;
			yield break;
		}

		if (pos < resources.Count) {
			// Use own resources first
			bool usable = false;
			foreach (Resource resource in resources[pos].Resources) {
				if (resourceCost.Contains(resource)) {
					usable = true;
					resourceCost.Remove(resource);
					foreach (Multiset<PlayerResource> set in GetBoughtResourceSets(pos + 1, resourceCost, toBuy)) {
						yield return set;
					}
					resourceCost.Add(resource);
				}
			}
			if (!usable) {
				foreach (Multiset<PlayerResource> set in GetBoughtResourceSets(pos + 1, resourceCost, toBuy)) {
					yield return set;
				}
			}

			yield break;
		}

		Dictionary<Direction, List<ResourceOptions>> neighbourResources =
			new Dictionary<Direction, List<ResourceOptions>>();
		neighbourResources[Direction.West] = Neighbours[Direction.West].ProducedResources;
		neighbourResources[Direction.East] = Neighbours[Direction.East].ProducedResources;

		Direction buyDirection = Direction.West;
		int resourcePos = 0;
		if (pos < resources.Count + neighbourResources[Direction.West].Count) {
			buyDirection = Direction.West;
			resourcePos = pos - resources.Count;
		} else if (pos < resources.Count + neighbourResources[Direction.West].Count + neighbourResources[Direction.East].Count) {
			buyDirection = Direction.East;
			resourcePos = pos - resources.Count - neighbourResources[Direction.West].Count;
		} else {
			yield break;
		}

		ResourceOptions resourceOptions = neighbourResources[buyDirection][resourcePos];
		foreach (Resource resource in resourceOptions.Resources) {
			if (!resourceCost.Contains(resource)) {
				continue;
			}

			PlayerResource playerResource = new PlayerResource(Neighbours[buyDirection], resource);
			resourceCost.Remove(resource);
			toBuy.Add(playerResource);
			foreach (Multiset<PlayerResource> set in GetBoughtResourceSets(pos + 1, resourceCost, toBuy)) {
				yield return set;
			}
			resourceCost.Add(resource);
			toBuy.Remove(playerResource);
		}

		foreach (Multiset<PlayerResource> set in GetBoughtResourceSets(pos + 1, resourceCost, toBuy)) {
			yield return set;
		}
	}

	protected int GetCheapestBoughtResources(
		Multiset<Resource> resourceCost, out Multiset<PlayerResource> cheapestBoughtResources
	) {
		int cheapestCost = Constant.MaxCost;
		cheapestBoughtResources = new Multiset<PlayerResource>();
		foreach (Multiset<PlayerResource> boughtResources in GetBoughtResourceSets(0, resourceCost, new Multiset<PlayerResource>())) {
			int cost = 0;
			foreach (PlayerResource playerResource in boughtResources) {
				cost += ResourceBuyCosts[playerResource];
			}
			if (cost < cheapestCost) {
				cheapestCost = cost;
				cheapestBoughtResources = new Multiset<PlayerResource>(boughtResources);
			}
		}
		
		return cheapestCost;
	}

	private void EnableBuildAreas(Card card) {
		if (BuiltCards.Contains(card.cardName)) {
			// Card has been built before, do not enable build area
			return;
		}

		bool chainable = false;
		foreach (string chainedCardName in card.chainedFrom) {
			if (BuiltCards.Contains(chainedCardName)) {
				chainable = true;
				break;
			}
		}

		if (chainable) {
			// Card is chainable, enable build area with 0 payment
			buildDropArea.payment = new Payment(0, 0, 0);
			buildDropArea.IsPlayable = true;
			return;
		}

		Multiset<Resource> cardResourceCost = new Multiset<Resource>(card.resourceCost);
		Multiset<PlayerResource> cheapestBoughtResources;
		int cheapestCost = card.coinCost + GetCheapestBoughtResources(cardResourceCost, out cheapestBoughtResources);
		if (cheapestCost <= bank.Count) {
			int westPayAmount = 0;
			int eastPayAmount = 0;
			foreach (PlayerResource boughtResource in cheapestBoughtResources) {
				if (boughtResource.player == Neighbours[Direction.West]) {
					westPayAmount += ResourceBuyCosts[boughtResource];
				} else if (boughtResource.player == Neighbours[Direction.East]) {
					eastPayAmount += ResourceBuyCosts[boughtResource];
				}
			}
			buildDropArea.payment = new Payment(card.coinCost, westPayAmount, eastPayAmount);
			buildDropArea.IsPlayable = true;
		}
	}

	private void EnableBuryAreas() {
		WonderStage[] buildableStages = Wonder.GetBuildableStages();
		foreach (WonderStage stage in buildableStages) {
			Multiset<Resource> stageResourceCost = new Multiset<Resource>(stage.resourceCost);
			Multiset<PlayerResource> cheapestBoughtResources;
			int cheapestCost = stage.coinCost + GetCheapestBoughtResources(stageResourceCost, out cheapestBoughtResources);
			if (cheapestCost <= bank.Count) {
				int westPayAmount = 0;
				int eastPayAmount = 0;
				foreach (PlayerResource boughtResource in cheapestBoughtResources) {
					if (boughtResource.player == Neighbours[Direction.West]) {
						westPayAmount += ResourceBuyCosts[boughtResource];
					} else if (boughtResource.player == Neighbours[Direction.East]) {
						eastPayAmount += ResourceBuyCosts[boughtResource];
					}
				}
				stage.buryDropArea.payment = new Payment(stage.coinCost, westPayAmount, eastPayAmount);
				stage.buryDropArea.IsPlayable = true;
			}
		}
	}

	private void EnableDiscardArea() {
		discardDropArea.IsPlayable = true;
	}

}
