public class Bot : Player {

	public override bool IsPlayable {
		set {
			if (value) {
				Action = null;
				// TODO: Simple AI
				DecideDiscard(hand.GetRandom());
			}
		}
	}

	public override void DecideBuild(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBotBuild(this, positionInHand);
	}

	public override void DecideBury(Card card, int wonderStage) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBotBury(this, positionInHand, wonderStage);
	}

	public override void DecideDiscard(Card card) {
		int positionInHand = hand.GetPosition(card);
		GameManager.Instance.DecideBotDiscard(this, positionInHand);
	}

}
