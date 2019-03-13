using UnityEngine;

public class MatchResolver : IResolver {
    
    public bool IsResolvable() {
		return true;
	}

    public void Resolve() {
		GameManager.Instance.EnqueueResolver(new PreGameResolver(), 1);
		GameManager.Instance.EnqueueResolver(new SelectLeadersResolver(), 1);
		GameManager.Instance.EnqueueResolver(new Age1Resolver(), 1);
		GameManager.Instance.EnqueueResolver(new Age2Resolver(), 1);
		GameManager.Instance.EnqueueResolver(new Age3Resolver(), 1);
		GameManager.Instance.EnqueueResolver(new PostGameResolver(), 1);
    }

}
