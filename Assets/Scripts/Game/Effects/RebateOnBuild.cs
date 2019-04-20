using System.Collections.Generic;

public class RebateOnBuild : OnBuildEffect {

	public Direction direction;
	public int amount;

	public RebateOnBuild(Direction direction, int amount) {
		this.direction = direction;
		this.amount = amount;
	}

	public override void Effect(Player player) {
		PaymentResolver.ModifyPayments rebate = (ref List<Payment> payments) => {
			for (int i = 0; i < payments.Count; i++) {
				payments[i].ApplyRebate(direction, amount);
			}
		};

		player.PaymentResolver.AddPaymentModifier(rebate);
	}

}
