using System.Collections.Generic;

public class FreeWonderOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		player.PaymentResolver.AddPaymentModifier(
			new PaymentModifier(
				delegate(Card cardToBuild, IEnumerable<Payment> payments) {
					return payments;
				},
				delegate(WonderStage stageToBuild, IEnumerable<Payment> payments) {
					return new Payment[] {
						new Payment(PaymentType.Normal, 0, 0, 0)
					};
				}
			)
		);
	}

}
