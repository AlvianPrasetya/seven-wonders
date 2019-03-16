using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LoadStockResolver : IResolvable {
	
	private StockType stockType;

	public LoadStockResolver(StockType stockType) {
		this.stockType = stockType;
	}

	public IEnumerator Resolve() {
		Stock stock;
		if (!GameManager.Instance.Stocks.TryGetValue(stockType, out stock)) {
			Debug.LogErrorFormat("Missing stock of type {0}", stockType);
			yield break;
		}

		yield return stock.Load();
	}

}
