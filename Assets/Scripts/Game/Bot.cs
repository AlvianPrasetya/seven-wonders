using System.Collections.Generic;

public class Bot : Player {

	public override void DecideDraft(Card card) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideBotDraft(this, positionInHand);
	}

	public override void DecideBuild(Card card, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideBotBuild(this, positionInHand, payment);
	}

	public override void DecideBury(Card card, int wonderStage, Payment payment) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideBotBury(this, positionInHand, wonderStage, payment);
	}

	public override void DecideDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		NetworkManager.Instance.DecideBotDiscard(this, positionInHand);
	}

}
