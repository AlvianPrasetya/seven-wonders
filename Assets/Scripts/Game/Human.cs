using System.Collections.Generic;

public class Human : Player {

	public override void DecideDraft(Card card) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideDraft(positionInHand);
	}

	public override void DecideBuild(Card card, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideBuild(positionInHand, payment);
	}

	public override void DecideBury(Card card, int wonderStage, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideBury(positionInHand, wonderStage, payment);
	}

	public override void DecideDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideDiscard(positionInHand);
	}

}
