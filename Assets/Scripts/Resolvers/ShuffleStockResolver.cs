using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ShuffleStockResolver : IResolvable {
	
	private StockType stockType;
	private int numIterations;
	private bool runInParallel;

	public ShuffleStockResolver(StockType stockType, int numIterations, bool runInParallel = false) {
		this.stockType = stockType;
		this.numIterations = numIterations;
		this.runInParallel = runInParallel;
	}

	public IEnumerator Resolve() {
		Stock stock;
		if (!GameManager.Instance.Stocks.TryGetValue(stockType, out stock)) {
			Debug.LogErrorFormat("Missing stock of type {0}", stockType);
			yield break;
		}

		if (runInParallel) {
			// Run in parallel if a monobehaviour is provided
			GameManager.Instance.StartCoroutine(stock.Shuffle(numIterations));
		} else {
			yield return stock.Shuffle(numIterations);
		}
	}

}
