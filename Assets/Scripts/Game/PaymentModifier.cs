using System;
using System.Collections.Generic;

public struct PaymentModifier {

	public Func<Card, IEnumerable<Payment>, IEnumerable<Payment>> ModifyForBuild {
		get; private set;
	}
	public Func<WonderStage, IEnumerable<Payment>, IEnumerable<Payment>> ModifyForBury {
		get; private set;
	}

	public PaymentModifier(
		Func<Card, IEnumerable<Payment>, IEnumerable<Payment>> modifyForBuild,
		Func<WonderStage, IEnumerable<Payment>, IEnumerable<Payment>> modifyForBury
	) {
		ModifyForBuild = modifyForBuild;
		ModifyForBury = modifyForBury;
	}

}
