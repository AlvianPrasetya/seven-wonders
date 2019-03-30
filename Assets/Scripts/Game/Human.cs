using System.Collections.Generic;

public class Human : Player {

	private List<Card> playableCards;

	public override bool IsPlayable {
		set {
			if (value) {
				playableCards = new List<Card>();
				foreach (CardPile displayPile in hand.cardPiles) {
					if (displayPile.Count != 0) {
						playableCards.Add(displayPile.Peek());
					}
				}

				foreach (Card card in playableCards) {
					card.IsPlayable = true;
				}

				Action = null;
			} else {
				foreach (Card card in playableCards) {
					card.IsPlayable = false;
				}

				if (Action == null) {
					// Player has yet to decide on an action, discard a random card
					DecideDiscard(hand.GetRandom());
				}
			}
		}
	}

	public override void DecideBuild(Card card, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBuild(positionInHand, payment);
	}

	public override void DecideBury(Card card, int wonderStage, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBury(positionInHand, wonderStage, payment);
	}

	public override void DecideDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideDiscard(positionInHand);
	}

}
