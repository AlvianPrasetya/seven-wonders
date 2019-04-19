using System;
using System.Collections;
using System.Collections.Generic;

public struct ConditionalResource {

	public Func<Player, Card, IEnumerable<Resource>> EvaluateForBuild {
		get; private set;
	}
	public Func<Player, WonderStage, IEnumerable<Resource>> EvaluateForBury {
		get; private set;
	}
	
	public ConditionalResource(
		Func<Player, Card, IEnumerable<Resource>> evaluateForBuild,
		Func<Player, WonderStage, IEnumerable<Resource>> evaluateForBury
	) {
		EvaluateForBuild = evaluateForBuild;
		EvaluateForBury = evaluateForBury;
	}

}
