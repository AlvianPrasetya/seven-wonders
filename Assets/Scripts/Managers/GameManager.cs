using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[System.Serializable]
	public class StockCardPile {

		public StockType stock;
		public Stock cardPile;

	}

	public StockCardPile[] stocks;

	public static GameManager Instance { get; private set; }
	public Dictionary<StockType, Stock> Stocks { get; private set; }
	public Stock DiscardPile { get; private set; }

	private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;

		Stocks = new Dictionary<StockType, Stock>();
		foreach (StockCardPile stockCardPile in stocks) {
			Stocks.Add(stockCardPile.stock, stockCardPile.cardPile);
		}
		resolverQueue = new ResolverQueue();
	}

	void Start() {
		resolverQueue.Enqueue(new MatchResolver(), 1);
		StartCoroutine(Resolve());
	}

	public void EnqueueResolver(IResolvable resolver, int priority) {
		resolverQueue.Enqueue(resolver, priority);
	}

	private IEnumerator Resolve() {
		while (resolverQueue.Size() != 0) {
			yield return resolverQueue.Dequeue().Resolve();
			Debug.Log("Queue size: " + resolverQueue.Size());
		}
	}

}