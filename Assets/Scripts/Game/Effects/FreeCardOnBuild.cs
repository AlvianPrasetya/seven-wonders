using System.Collections.Generic;

public class FreeCardOnBuild : OnBuildEffect {

	public CardType cardType;

	public override void Effect(Player player) {
		player.PaymentResolver.AddPaymentModifier(
			new PaymentModifier(
				delegate(Card cardToBuild, IEnumerable<Payment> payments) {
					if (cardToBuild.cardType == cardType) {
						return new Payment[] {
							new Payment(PaymentType.Normal, 0, 0, 0)
						};
					}
					
					return payments;
				},
				delegate(WonderStage stageToBuild, IEnumerable<Payment> payments) {
					return payments;
				}
			)
		);
	}

}
