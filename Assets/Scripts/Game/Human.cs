public class Human : Player {

	public override bool IsPlayable {
		set {
			foreach (CardPile displayPile in hand.cardPiles) {
				if (displayPile.Count != 0) {
					displayPile.Peek().IsPlayable = value;
				}
			}

			buildDropArea.IsPlayable = value;
			discardDropArea.IsPlayable = value;
			Wonder.IsPlayable = value;

			if (value) {
				Action = null;
			} else {
				if (Action == null) {
					// Player has yet to decide on an action, discard a random card
					DecideDiscard(hand.GetRandom());
				}
			}
		}
	}

	public override void DecideBuild(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBuild(positionInHand);
	}

	public override void DecideBury(Card card, int wonderStage) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBury(positionInHand, wonderStage);
	}

	public override void DecideDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideDiscard(positionInHand);
	}

}
