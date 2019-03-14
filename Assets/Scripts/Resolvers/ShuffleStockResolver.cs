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
	}

}
