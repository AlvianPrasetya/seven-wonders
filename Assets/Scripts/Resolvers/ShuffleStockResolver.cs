using System.Collections.Generic;
using UnityEngine;

public class ShuffleStockResolver : IResolver {
	
	private StockType stockToShuffle;

	public ShuffleStockResolver(StockType stockToShuffle) {
		this.stockToShuffle = stockToShuffle;
	}

	public bool IsResolvable() {
		return GameManager.Instance.StockPiles.ContainsKey(stockToShuffle);
	}

	public void Resolve() {
		Stock stock;
		if (!GameManager.Instance.StockPiles.TryGetValue(stockToShuffle, out stock)) {
			return;
		}
	}

}
