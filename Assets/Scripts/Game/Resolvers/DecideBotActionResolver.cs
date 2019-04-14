using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideBotActionResolver : IResolvable {

	private Bot bot;

	public DecideBotActionResolver(Bot bot) {
		this.bot = bot;
	}

	public IEnumerator Resolve() {
		bot.IsPlayable = true;

		yield return Decide();

		bot.IsPlayable = false;

		yield return null;
	}

	private IEnumerator Decide() {
		// Build the first buildable wonder stage
		WonderStage[] buildableStages = bot.Wonder.GetBuildableStages();
		foreach (WonderStage stage in buildableStages) {
			List<Payment> possiblePayments = new List<Payment>(
				bot.PaymentResolver.Resolve(bot, stage, bot.hand.PlayableCards[0])
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

			if (cheapestPayment.TotalAmount <= bot.bank.Count) {
				bot.DecideBury(bot.hand.PlayableCards[0], stage.buryDropArea.wonderStage, cheapestPayment);
				yield break;
			}
		}

		// Build the first buildable card in current hand
		foreach (Card card in bot.hand.PlayableCards) {
			if (bot.BuiltCards.Contains(card.cardName)) {
				// Can't build duplicate cards
				continue;
			}

			List<Payment> possiblePayments = new List<Payment>(
				bot.PaymentResolver.Resolve(bot, card)
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

			if (cheapestPayment.TotalAmount <= bot.bank.Count) {
				bot.DecideBuild(card, cheapestPayment);
				yield break;
			}
		}

		// Discard any card if unable to build any
		bot.DecideDiscard(bot.hand.GetRandom());
	}

}
