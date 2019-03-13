using UnityEngine;

public class MatchResolver : IResolver {
    
    public bool IsResolvable() {
		return true;
	}

    public void Resolve() {
		GameManager.Instance.EnqueueResolver(new PreGameResolver(), 4);
		GameManager.Instance.EnqueueResolver(new SelectLeadersResolver(), 6);
		GameManager.Instance.EnqueueResolver(new Age1Resolver(), 3);
		GameManager.Instance.EnqueueResolver(new Age2Resolver(), 1);
		GameManager.Instance.EnqueueResolver(new Age3Resolver(), 5);
		GameManager.Instance.EnqueueResolver(new PostGameResolver(), 2);
    }

}
