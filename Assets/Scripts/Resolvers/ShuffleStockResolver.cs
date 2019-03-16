using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ShuffleStockResolver : IResolvable {
	
	private StockType stockType;
	private int numIterations;

	public ShuffleStockResolver(StockType stockType, int numIterations) {
		this.stockType = stockType;
		this.numIterations = numIterations;
	}

	public IEnumerator Resolve() {
		Stock stock;
		if (!GameManager.Instance.Stocks.TryGetValue(stockType, out stock)) {
			Debug.LogErrorFormat("Missing stock of type {0}", stockType);
			yield break;
		}

		yield return stock.Shuffle(numIterations);
	}

}
