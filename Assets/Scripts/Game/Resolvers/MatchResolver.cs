using System.Collections;
using UnityEngine;

public class MatchResolver : IResolvable {

	private int matchSeed;

	public MatchResolver(int matchSeed) {
		this.matchSeed = matchSeed;
	}
	
	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new PreGameResolver(matchSeed), Priority.ResolvePhase);
		GameManager.Instance.EnqueueResolver(new Age1Resolver(), Priority.ResolvePhase);
		GameManager.Instance.EnqueueResolver(new Age2Resolver(), Priority.ResolvePhase);
		GameManager.Instance.EnqueueResolver(new Age3Resolver(), Priority.ResolvePhase);
		GameManager.Instance.EnqueueResolver(new PostGameResolver(), Priority.ResolvePhase);

		yield return null;
	}

}
