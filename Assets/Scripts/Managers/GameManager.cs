using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameManager Instance { get; private set; }
	private Queue<IResolver> resolverQueue;
	public Queue<Card> discardPile { get; private set; }

	void Awake() {
		Instance = this;
		resolverQueue = new Queue<IResolver>();
		discardPile = new Queue<Card>();
	}

	void Start() {
		resolverQueue.Enqueue(new MatchResolver());
	}

	void Update() {
		Resolve();
	}

	public void EnqueueResolver(IResolver resolver) {
		resolverQueue.Enqueue(resolver);
	}

	public void AddToDiscardPile(Card cardToDiscard) {
		discardPile.Enqueue(cardToDiscard);
	}

	private void Resolve() {
		if (resolverQueue.Count == 0) {
			// No resolver pending in queue, stop resolving
			return;
		}

		IResolver resolver = resolverQueue.Dequeue();
		resolver.Resolve();
	}

}