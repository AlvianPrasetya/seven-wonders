using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[System.Serializable]
	public class StockEntry {

		public StockType stockType;
		public Stock stock;

	}

	public GameCamera gameCamera;
	public Bank bank;
	public Deck discardPile;
	public StockEntry[] stocks;
	public Player[] players;

	public static GameManager Instance { get; private set; }
	public Dictionary<StockType, Stock> Stocks { get; private set; }

	private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;

		Stocks = new Dictionary<StockType, Stock>();
		foreach (StockEntry stockEntry in stocks) {
			Stocks.Add(stockEntry.stockType, stockEntry.stock);
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