using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[System.Serializable]
	public class StockCardPile {

		public Stock stock;
		public CardPile cardPile;

	}

	public static GameManager Instance { get; private set; }

	public CardPile DiscardPile { get; private set; }
	public Dictionary<Stock, CardPile> StockPiles { get; private set; }

	[SerializeField]
	private StockCardPile[] stocks;
	private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;

		StockPiles = new Dictionary<Stock, CardPile>();
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