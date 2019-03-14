using UnityEngine;

public class DealStockResolver : IResolver {
    
	private Stock stockToDeal;

	public DealStockResolver(Stock stockToDeal) {
		this.stockToDeal = stockToDeal;
	}

    public bool IsResolvable() {
		return GameManager.Instance.StockPiles.ContainsKey(stockToDeal);
	}

    public void Resolve() {
    }

}
