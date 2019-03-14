using UnityEngine;

public class PreGameResolver : IResolver {

	public bool IsResolvable() {
		return true;
	}

	public void Resolve() {
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(Stock.Guild), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(Stock.City), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(Stock.Leader), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(Stock.Wonder), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge3StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge2StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge1StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(Stock.Age3), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(Stock.Age2), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(Stock.Age1), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(Stock.Age3), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(Stock.Age2), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(Stock.Age1), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(Stock.Leader), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(Stock.Wonder), 3);
	}

}
