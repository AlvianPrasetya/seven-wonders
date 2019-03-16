using System.Collections;
using UnityEngine;

public class DealStockResolver : IResolvable {
	
	private StockType stockToDeal;

	public DealStockResolver(StockType stockToDeal) {
		this.stockToDeal = stockToDeal;
	}

	public IEnumerator Resolve() {
		yield return null;
	}

}
