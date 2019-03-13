using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }
    private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;
        resolverQueue = new ResolverQueue();
	}

	void Start() {
		resolverQueue.Enqueue(new MatchResolver(), 1);
	}

	void Update() {
		Resolve();
	}

	public void EnqueueResolver(IResolver resolver, int priority) {
		resolverQueue.Enqueue(resolver, priority);
	}

	private void Resolve() {
		if (resolverQueue.Size() == 0) {
			// No resolver pending in queue, stop resolving
			return;
		}

		IResolver resolver = resolverQueue.Peek();
		if (resolver.TryResolve()) {
			resolverQueue.Dequeue();
		}

        Debug.Log("Queue size: " + resolverQueue.Size());
	}

}