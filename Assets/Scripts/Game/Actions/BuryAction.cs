using System.Collections;

public class BuryAction : IActionable {

	private Card card;
	private int wonderStage;
	private Payment payment;

	public BuryAction(Card card, int wonderStage, Payment payment) {
		this.card = card;
		this.wonderStage = wonderStage;
		this.payment = payment;
	}

	public IEnumerator Perform(Player player) {
		yield return player.Wonder.wonderStages[wonderStage].buildCardSlot.Push(card);

		yield return player.Pay(payment);
	}

	public void Effect(Player player) {
		foreach (OnBuildEffect onBuildEffect in player.Wonder.wonderStages[wonderStage].onBuildEffects) {
			onBuildEffect.Effect(player);
		}
	}

}
