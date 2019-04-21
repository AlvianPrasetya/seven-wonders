using System;
using System.Collections.Generic;

public class RebateOnBuild : OnBuildEffect {

	public Direction direction;
	public int amount;

	public RebateOnBuild(Direction direction, int amount) {
		this.direction = direction;
		this.amount = amount;
	}

	public override void Effect(Player player) {
		player.PaymentResolver.AddPaymentModifier(
			new PaymentModifier(
				delegate(Card cardToBuild, IEnumerable<Payment> payments) {
					List<Payment> rebatedPayments = new List<Payment>();

					foreach (Payment payment in payments) {
						int payWestAmount = payment.PayWestAmount;
						int payEastAmount = payment.PayEastAmount;
						switch (direction) {
							case Direction.West:
								payWestAmount = Math.Max(
									payWestAmount - amount,
									0
								);
								break;
							case Direction.East:
								payEastAmount = Math.Max(
									payEastAmount - amount,
									0
								);
								break;
						}
						rebatedPayments.Add(new Payment(
							payment.PaymentType,
							payment.PayBankAmount,
							payWestAmount,
							payEastAmount
						));
					}

					return rebatedPayments;
				},
				delegate(WonderStage stageToBuild, IEnumerable<Payment> payments) {
					List<Payment> rebatedPayments = new List<Payment>();
					
					foreach (Payment payment in payments) {
						int payWestAmount = payment.PayWestAmount;
						int payEastAmount = payment.PayEastAmount;
						switch (direction) {
							case Direction.West:
								payWestAmount = Math.Max(
									payWestAmount - amount,
									0
								);
								break;
							case Direction.East:
								payEastAmount = Math.Max(
									payEastAmount - amount,
									0
								);
								break;
						}
						rebatedPayments.Add(new Payment(
							payment.PaymentType,
							payment.PayBankAmount,
							payWestAmount,
							payEastAmount
						));
					}

					return rebatedPayments;
				}
			)
		);
	}

}
