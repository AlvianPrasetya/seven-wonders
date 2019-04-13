using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
