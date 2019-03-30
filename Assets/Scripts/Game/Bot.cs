using System.Collections.Generic;

public class Bot : Player {

	public override bool IsPlayable {
		set {
			if (value) {
				Action = null;

				List<Card> playableCards = new List<Card>();
				foreach (CardPile displayPile in hand.cardPiles) {
					if (displayPile.Count != 0) {
						playableCards.Add(displayPile.Peek());
					}
				}

				// Build the first buildable card in current hand
				foreach (Card card in playableCards) {
					Multiset<Resource> cardResourceCost = new Multiset<Resource>(card.resourceCost);

					Multiset<PlayerResource> cheapestBoughtResources;
					int cheapestCost = card.coinCost + GetCheapestBoughtResources(cardResourceCost, out cheapestBoughtResources);
					if (cheapestCost > bank.Count) {
						// Could not afford to build this card
						continue;
					}

					int westPayAmount = 0;
					int eastPayAmount = 0;
					foreach (PlayerResource boughtResource in cheapestBoughtResources) {
						if (boughtResource.player == Neighbours[Direction.West]) {
							westPayAmount += ResourceBuyCosts[boughtResource];
						} else if (boughtResource.player == Neighbours[Direction.East]) {
							eastPayAmount += ResourceBuyCosts[boughtResource];
						}
					}

					DecideBuild(card, new Payment(card.coinCost, westPayAmount, eastPayAmount));
					return;
				}

				// Discard any card if unable to build any
				DecideDiscard(hand.GetRandom());
			}
		}
	}

	public override void DecideBuild(Card card, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBotBuild(this, positionInHand, payment);
	}

	public override void DecideBury(Card card, int wonderStage, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBotBury(this, positionInHand, wonderStage, payment);
	}

	public override void DecideDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBotDiscard(this, positionInHand);
	}

}
