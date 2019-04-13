using System.Collections.Generic;

public class Bot : Player {

	public override bool IsPlayable {
		set {
			if (value) {
				hand.IsPlayable = true;

				// Build the first buildable wonder stage
				WonderStage[] buildableStages = Wonder.GetBuildableStages();
				foreach (WonderStage stage in buildableStages) {
					List<Payment> possiblePayments = new List<Payment>(
						PaymentResolver.Instance.Resolve(this, stage, hand.PlayableCards[0])
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
						DecideBury(hand.PlayableCards[0], stage.buryDropArea.wonderStage, cheapestPayment);
						return;
					}
				}

				// Build the first buildable card in current hand
				foreach (Card card in hand.PlayableCards) {
					if (BuiltCards.Contains(card.cardName)) {
						// Can't build duplicate cards
						continue;
					}

					List<Payment> possiblePayments = new List<Payment>(
						PaymentResolver.Instance.Resolve(this, card)
					);
					if (possiblePayments.Count == 0) {
						// No payment combinations possible for this card
						continue;
					}

					Payment cheapestPayment = possiblePayments[0];
					foreach (Payment payment in possiblePayments) {
						if (payment < cheapestPayment) {
							cheapestPayment = payment;
						}
					}

					if (cheapestPayment.TotalAmount <= bank.Count) {
						DecideBuild(card, cheapestPayment);
						return;
					}
				}

				// Discard any card if unable to build any
				DecideDiscard(hand.GetRandom());
			} else {
				hand.IsPlayable = false;
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
