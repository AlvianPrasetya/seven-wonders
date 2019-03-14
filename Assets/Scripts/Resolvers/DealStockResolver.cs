using UnityEngine;

public class DealStockResolver : IResolver {
	
	private StockType stockToDeal;

	public DealStockResolver(StockType stockToDeal) {
		this.stockToDeal = stockToDeal;
	}

	public bool IsResolvable() {
		return GameManager.Instance.StockPiles.ContainsKey(stockToDeal);
	}

	public void Resolve() {
	}

}
