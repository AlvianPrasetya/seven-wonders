using UnityEngine;

public class MatchResolver : IResolver {
    
    public bool IsResolvable() {
		return true;
	}

    public void Resolve() {
		GameManager.Instance.EnqueueResolver(new PreGameResolver(), 2);
		GameManager.Instance.EnqueueResolver(new SelectLeadersResolver(), 2);
		GameManager.Instance.EnqueueResolver(new Age1Resolver(), 2);
		GameManager.Instance.EnqueueResolver(new Age2Resolver(), 2);
		GameManager.Instance.EnqueueResolver(new Age3Resolver(), 2);
		GameManager.Instance.EnqueueResolver(new PostGameResolver(), 2);
    }

}
