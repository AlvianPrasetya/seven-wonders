public class Human : Player {

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
