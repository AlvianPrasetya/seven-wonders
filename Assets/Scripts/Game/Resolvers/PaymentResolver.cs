using System;
using System.Collections;
using System.Collections.Generic;

public class PaymentResolver : IResolvable {

	private static PaymentResolver instance;

	public static PaymentResolver Instance {
		get {
			if (instance == null) {
				instance = new PaymentResolver();
			}

			return instance;
		}
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

		HashSet<Payment> possiblePayments = new HashSet<Payment>(GetPaymentCombinations(
			GetOwnedResources(player),
			GetBuyableResources(player),
			new Multiset<Resource>(cardToBuild.resourceCost),
			new Payment(PaymentType.Normal, cardToBuild.coinCost, 0, 0)
		));

		return possiblePayments;
	}

	public IEnumerable<Payment> Resolve(Player player, WonderStage stageToBuild, Card cardToBury) {
		HashSet<Payment> possiblePayments = new HashSet<Payment>(GetPaymentCombinations(
			GetOwnedResources(player),
			GetBuyableResources(player),
			new Multiset<Resource>(stageToBuild.resourceCost),
			new Payment(PaymentType.Normal, stageToBuild.coinCost, 0, 0)
		));

		return possiblePayments;
	}
	
	private Multiset<ResourceOptions> GetOwnedResources(Player player) {
		Multiset<ResourceOptions> ownedResources = new Multiset<ResourceOptions>();

		ownedResources.Add(player.resources);

		return ownedResources;
	}

	private Multiset<BuyableResourceOptions> GetBuyableResources(Player player) {
		Multiset<BuyableResourceOptions> buyableResources = new Multiset<BuyableResourceOptions>();

		// Evaluate buyable resources from west neighbour
		List<ResourceOptions> westResources = player.Neighbours[Direction.West].ProducedResources;
		foreach (ResourceOptions resource in westResources) {
			int buyCost = player.ResourceBuyCosts[
				new KeyValuePair<Direction, Resource>(Direction.West, resource.Resources[0])
			];
			
			buyableResources.Add(
				new BuyableResourceOptions(resource, new Payment(PaymentType.Normal, 0, buyCost, 0))
			);
		}

		// Evaluate buyable resources from east neighbour
		List<ResourceOptions> eastResources = player.Neighbours[Direction.East].ProducedResources;
		foreach (ResourceOptions resource in eastResources) {
			int buyCost = player.ResourceBuyCosts[
				new KeyValuePair<Direction, Resource>(Direction.East, resource.Resources[0])
			];
			
			buyableResources.Add(
				new BuyableResourceOptions(resource, new Payment(PaymentType.Normal, 0, 0, buyCost))
			);
		}

		return buyableResources;
	}

	private IEnumerable<Payment> GetPaymentCombinations(
		Multiset<ResourceOptions> ownedResources,
		Multiset<BuyableResourceOptions> buyableResources,
		Multiset<Resource> resourceCost,
		Payment currentPayment
	) {
		if (resourceCost.Count == 0) {
			yield return currentPayment;
			yield break;
		}

		// Use owned resources first
		if (ownedResources.Count != 0) {
			ResourceOptions resourceOptions = ownedResources.Pop();
			bool usable = false;
			foreach (Resource resource in resourceOptions.Resources) {
				if (!resourceCost.Contains(resource)) {
					// Choosing this resource does not contribute towards fulfilling the cost
					continue;
				}
				
				usable = true;
				resourceCost.Remove(resource);
				foreach (Payment payment in GetPaymentCombinations(
					ownedResources,
					buyableResources,
					resourceCost,
					currentPayment
				)) {
					yield return payment;
				}
				resourceCost.Add(resource);
			}
			if (!usable) {
				// Any resource choices do not contribute towards fulfilling the cost
				foreach (Payment payment in GetPaymentCombinations(
					ownedResources,
					buyableResources,
					resourceCost,
					currentPayment
				)) {
					yield return payment;
				}
			}
			ownedResources.Add(resourceOptions);
		}

		// Resort to buyable resources if self-fulfilment is not possible
		if (buyableResources.Count != 0) {
			BuyableResourceOptions buyableResourceOptions = buyableResources.Pop();
			bool usable = false;
			foreach (Resource resource in buyableResourceOptions.ResourceOptions.Resources) {
				if (!resourceCost.Contains(resource)) {
					// Buying this resource does not contribute towards fulfilling the cost
					continue;
				}

				usable = true;
				resourceCost.Remove(resource);
				foreach (Payment payment in GetPaymentCombinations(
					ownedResources,
					buyableResources,
					resourceCost,
					currentPayment + buyableResourceOptions.Cost
				)) {
					yield return payment;
				}
				resourceCost.Add(resource);
			}
			if (!usable) {
				// Any resource choices do not contribute towards fulfilling the cost
				foreach (Payment payment in GetPaymentCombinations(
					ownedResources,
					buyableResources,
					resourceCost,
					currentPayment
				)) {
					yield return payment;
				}
			}
			buyableResources.Add(buyableResourceOptions);
		}

		// This combination of resources does not fulfill the cost
		yield break;
	}

}
