using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[System.Serializable]
	public class StockCardPile {

		public StockType stock;
		public Stock cardPile;

	}

	public static GameManager Instance { get; private set; }

	public StockCardPile[] stocks;

	public Stock DiscardPile { get; private set; }
	public Dictionary<StockType, Stock> StockPiles { get; private set; }
	private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;

		StockPiles = new Dictionary<StockType, Stock>();
		foreach (StockCardPile stockCardPile in stocks) {
			StockPiles.Add(stockCardPile.stock, stockCardPile.cardPile);
		}
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
		if (resolver.IsResolvable()) {
			resolverQueue.Dequeue();
			resolver.Resolve();
		}

		Debug.Log("Queue size: " + resolverQueue.Size());
	}

}