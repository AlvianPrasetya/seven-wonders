using System;
using System.Collections;
using System.Collections.Generic;

public struct ConditionalResourceOptions {

	public Func<Player, Card, IEnumerable<ResourceOptions>> EvaluateForBuild {
		get; private set;
	}
	public Func<Player, WonderStage, IEnumerable<ResourceOptions>> EvaluateForBury {
		get; private set;
	}
	
	public ConditionalResourceOptions(
		Func<Player, Card, IEnumerable<ResourceOptions>> evaluateForBuild,
		Func<Player, WonderStage, IEnumerable<ResourceOptions>> evaluateForBury
	) {
		EvaluateForBuild = evaluateForBuild;
		EvaluateForBury = evaluateForBury;
	}

}
