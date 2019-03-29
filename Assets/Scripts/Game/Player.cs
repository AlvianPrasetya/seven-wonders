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
				new GainCoinsResolver(player, GameOptions.DiscardCoinAmount), 4
			);
		}

	}

	public delegate IEnumerator TurnAction(
		Card card, params object[] args
	);

	private struct PlayerResource {
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
	public Dictionary<CardType, List<Card>> BuiltCardsByType { get; private set; }
	public string Nickname { get; set; }
	public List<ResourceOptions> ProducedResources {
		get {
			List<ResourceOptions> producedResources = new List<ResourceOptions>();
			foreach (ResourceOptions resource in resources) {
				if (resource.produced) {
					producedResources.Add(resource);
				}
			}

			return producedResources;
		}
	}

	private List<ResourceOptions> resources;

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
		resources = new List<ResourceOptions>();
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

	public void AddResource(ResourceOptions resourceOptions) {
		resources.Add(resourceOptions);
	}

	public void EvaluatePlayability(Card card) {
		Multiset<Resource> cardResourceCost = new Multiset<Resource>(card.resourceCost);
		foreach (Multiset<PlayerResource> boughtResourceSet in GetBoughtResourceSets(0, cardResourceCost, new Multiset<PlayerResource>())) {
			string str = "";
			foreach (PlayerResource boughtResource in boughtResourceSet) {
				str += string.Format("Buy {0} from {1}, ", boughtResource.resource, boughtResource.player.Nickname);
			}
			Debug.Log(str);
		}
	}

	// TODO: Refactor
	private IEnumerable<Multiset<PlayerResource>> GetBoughtResourceSets(
		int pos, Multiset<Resource> resourceCost, Multiset<PlayerResource> toBuy
	) {
		if (resourceCost.Count == 0) {
			yield return toBuy;
			yield break;
		}

		if (pos < resources.Count) {
			// Use own resources first
			bool usable = false;
			foreach (Resource resource in resources[pos].resources) {
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
		foreach (Resource resource in resourceOptions.resources) {
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

}
