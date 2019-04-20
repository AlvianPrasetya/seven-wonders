using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PaymentResolver : IResolvable {
	
	public delegate void ModifyPayments(ref List<Payment> payments);

	private Player player;
	// Statically allocated processing variables to optimize memory usage
	private List<ResourceType> resourceCost;
	private List<Resource> ownedResources;
	private List<BuyableResource> buyableResources;
	private Dictionary<KeyValuePair<int, int>, HashSet<Payment>> memo;
	private List<ModifyPayments> paymentModifiers;

	public PaymentResolver(Player player) {
		this.player = player;
		resourceCost = new List<ResourceType>();
		ownedResources = new List<Resource>();
		buyableResources = new List<BuyableResource>();
		memo = new Dictionary<KeyValuePair<int, int>, HashSet<Payment>>();
	}

	public IEnumerator Resolve() {
		yield return new System.NotImplementedException();
	}

	public IEnumerable<Payment> Resolve(Player player, Card cardToBuild) {
		foreach (string chainedCardName in cardToBuild.chainedFrom) {
			if (player.BuiltCards.Contains(chainedCardName)) {
				// Card is chainable for 0 cost
				return new Payment[]{
					new Payment(PaymentType.Chained, 0, 0, 0)
				};
			}
		}

		EvaluateResourceCost(cardToBuild);
		EvaluateOwnedResources(player, cardToBuild);
		EvaluateBuyableResources(player);
		memo.Clear();

		List<Payment> payments = new List<Payment>(
			GetPaymentCombinations((1 << resourceCost.Count) - 1)
		);
		// Add in card base coin cost
		for (int i = 0; i < payments.Count; i++) {
			payments[i] += new Payment(PaymentType.Normal, cardToBuild.coinCost, 0, 0);
		}
		// Apply payment modifiers
		foreach (ModifyPayments paymentModifier in paymentModifiers) {
			paymentModifier.Invoke(ref payments);
		}

		return payments;
	}

	public IEnumerable<Payment> Resolve(Player player, WonderStage stageToBuild, Card cardToBury) {
		EvaluateResourceCost(stageToBuild);
		EvaluateOwnedResources(player, stageToBuild);
		EvaluateBuyableResources(player);
		memo.Clear();

		List<Payment> payments = new List<Payment>(
			GetPaymentCombinations((1 << resourceCost.Count) - 1)
		);
		// Add in wonder stage base coin cost
		for (int i = 0; i < payments.Count; i++) {
			payments[i] += new Payment(PaymentType.Normal, stageToBuild.coinCost, 0, 0);
		}
		// Apply payment modifiers
		foreach (ModifyPayments paymentModifier in paymentModifiers) {
			paymentModifier.Invoke(ref payments);
		}

		return payments;
	}

	public void AddPaymentModifier(ModifyPayments paymentModifier) {
		paymentModifiers.Add(paymentModifier);
	}

	private void EvaluateResourceCost(Card cardToBuild) {
		resourceCost.Clear();

		foreach (ResourceType resource in cardToBuild.resourceCost) {
			resourceCost.Add(resource);
		}
	}

	private void EvaluateResourceCost(WonderStage stageToBuild) {
		resourceCost.Clear();

		foreach (ResourceType resource in stageToBuild.resourceCost) {
			resourceCost.Add(resource);
		}
	}
	
	private void EvaluateOwnedResources(Player player, Card cardToBuild) {
		ownedResources.Clear();

		foreach (Resource resource in player.Resources) {
			ownedResources.Add(resource);
		}

		foreach (ConditionalResource conditionalResource in player.ConditionalResources) {
			foreach (Resource resource in conditionalResource.EvaluateForBuild(
				player, cardToBuild
			)) {
				ownedResources.Add(resource);
			}
		}
	}

	private void EvaluateOwnedResources(Player player, WonderStage stageToBuild) {
		ownedResources.Clear();

		foreach (Resource resource in player.Resources) {
			ownedResources.Add(resource);
		}

		foreach (ConditionalResource conditionalResource in player.ConditionalResources) {
			foreach (Resource resource in conditionalResource.EvaluateForBury(
				player, stageToBuild
			)) {
				ownedResources.Add(resource);
			}
		}
	}

	private void EvaluateBuyableResources(Player player) {
		buyableResources.Clear();

		// Evaluate buyable resources from west neighbour
		List<Resource> westResources = player.Neighbours[Direction.West].ProducedResources;
		foreach (Resource resource in westResources) {
			int buyCost = player.ResourceBuyCosts[
				new KeyValuePair<Direction, ResourceType>(Direction.West, resource.ResourceTypes[0])
			];
			
			buyableResources.Add(
				new BuyableResource(resource, new Payment(PaymentType.Normal, 0, buyCost, 0))
			);
		}

		// Evaluate buyable resources from east neighbour
		List<Resource> eastResources = player.Neighbours[Direction.East].ProducedResources;
		foreach (Resource resource in eastResources) {
			int buyCost = player.ResourceBuyCosts[
				new KeyValuePair<Direction, ResourceType>(Direction.East, resource.ResourceTypes[0])
			];
			
			buyableResources.Add(
				new BuyableResource(resource, new Payment(PaymentType.Normal, 0, 0, buyCost))
			);
		}
	}

	private IEnumerable<Payment> GetPaymentCombinations(
		int costBitmask,
		int pos = 0
	) {
		// Base case #1: All resource costs are fulfilled
		if (costBitmask == 0) {
			yield return new Payment();
			yield break;
		}

		// Base case #2: This combination has been evaluated and memoized
		HashSet<Payment> memoizedPayments;
		if (memo.TryGetValue(new KeyValuePair<int, int>(costBitmask, pos), out memoizedPayments)) {
			foreach (Payment payment in memoizedPayments) {
				yield return payment;
			}
			yield break;
		}

		if (pos < ownedResources.Count) {
			// Use owned resources first
			int actualPos = pos;
			bool usable = false;
			foreach (ResourceType resourceType in ownedResources[actualPos].ResourceTypes) {
				for (int i = 0; i < resourceCost.Count; i++) {
					if ((costBitmask & (1 << i)) == 0 || resourceType != resourceCost[i]) {
						// This resource is no longer required or resources do not match
						continue;
					}

					// Matching a required resource, further evaluate this combination
					usable = true;
					Memoize(costBitmask, pos, GetPaymentCombinations(
						costBitmask ^ (1 << i), // Remove this resource from cost
						pos + 1
					));
				}
			}
			if (!usable) {
				// Any resource choices do not contribute towards fulfilling the cost,
				// further evaluate without using the current owned resource
				Memoize(costBitmask, pos, GetPaymentCombinations(
					costBitmask,
					pos + 1
				));
			}

			foreach (Payment payment in Recall(costBitmask, pos)) {
				yield return payment;
			}
		} else if (pos < ownedResources.Count + buyableResources.Count) {
			// Resort to buyable resources if self-fulfilment is not possible
			int actualPos = pos - ownedResources.Count;
			BuyableResource buyableResource = buyableResources[actualPos];
			foreach (ResourceType resource in buyableResource.Resource.ResourceTypes) {
				for (int i = 0; i < resourceCost.Count; i++) {
					if ((costBitmask & (1 << i)) == 0 || resource != resourceCost[i]) {
						// This resource is no longer required or resources do not match
						continue;
					}

					// Matching a required resource, further evaluate this combination
					Memoize(costBitmask, pos, GetPaymentCombinations(
						costBitmask ^ (1 << i), // Remove this resource from cost
						pos + 1
					), buyableResource.Cost);
				}
			}
			Memoize(costBitmask, pos, GetPaymentCombinations(
				costBitmask,
				pos + 1
			));

			foreach (Payment payment in Recall(costBitmask, pos)) {
				yield return payment;
			}
		} else {
			// This combination of owned and buyable resources does not fulfill the cost
			yield break;
		}
	}

	private void Memoize(int costBitmask, int pos, IEnumerable<Payment> payments, Payment addedCost = new Payment()) {
		KeyValuePair<int, int> key = new KeyValuePair<int, int>(costBitmask, pos);
		if (!memo.ContainsKey(key)) {
			memo[key] = new HashSet<Payment>();
		}

		foreach (Payment payment in payments) {
			memo[key].Add(payment + addedCost);
		}
	}

	private IEnumerable<Payment> Recall(int costBitmask, int pos) {
		KeyValuePair<int, int> key = new KeyValuePair<int, int>(costBitmask, pos);
		HashSet<Payment> payments;
		if (memo.TryGetValue(key, out payments)) {
			return payments;
		}

		return Enumerable.Empty<Payment>();
	}

}
