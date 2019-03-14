using UnityEngine;

public class ShuffleStockResolver : IResolver {
	
	private Stock stockToShuffle;

	public ShuffleStockResolver(Stock stockToShuffle) {
		this.stockToShuffle = stockToShuffle;
	}

	public bool IsResolvable() {
		return GameManager.Instance.StockPiles.ContainsKey(stockToShuffle);
	}

	public void Resolve() {
	}

}
