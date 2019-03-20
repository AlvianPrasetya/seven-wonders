using System.Collections;
using UnityEngine;

public class MatchResolver : IResolvable {

	private int matchSeed;

	public MatchResolver(int matchSeed) {
		this.matchSeed = matchSeed;
	}
	
	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new PreGameResolver(matchSeed), 2);
		GameManager.Instance.EnqueueResolver(new Age1Resolver(), 2);
		GameManager.Instance.EnqueueResolver(new Age2Resolver(), 2);
		GameManager.Instance.EnqueueResolver(new Age3Resolver(), 2);
		GameManager.Instance.EnqueueResolver(new PostGameResolver(), 2);

		yield return null;
	}

}
