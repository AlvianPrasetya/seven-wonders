using UnityEngine;

public class MatchResolver : IResolver {

	const int numAges = 3;
	
	public void Resolve() {
		GameManager.Instance.EnqueueResolver(new SelectLeadersResolver());
		for (int i = 1; i <= numAges; i++) {
			GameManager.Instance.EnqueueResolver(new AgeResolver(i));
		}
		GameManager.Instance.EnqueueResolver(new TallyPointsResolver());
	}

}
