using UnityEngine;

public class SelectLeadersResolver : IResolver {
	
	public void Resolve() {
		GameManager.Instance.EnqueueResolver(new SelectLeaderResolver());
		GameManager.Instance.EnqueueResolver(new PassHandResolver(Direction.Right));
		GameManager.Instance.EnqueueResolver(new SelectLeaderResolver());
		GameManager.Instance.EnqueueResolver(new PassHandResolver(Direction.Right));
		GameManager.Instance.EnqueueResolver(new SelectLeaderResolver());
		GameManager.Instance.EnqueueResolver(new PassHandResolver(Direction.Right));
		GameManager.Instance.EnqueueResolver(new SelectLeaderResolver());
		GameManager.Instance.EnqueueResolver(new DiscardLastResolver());
	}

}
