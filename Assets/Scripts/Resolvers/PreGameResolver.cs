using UnityEngine;

public class PreGameResolver : IResolver {

	public bool IsResolvable() {
		return true;
	}

	public void Resolve() {
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Guild), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.City), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Leader), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Wonder), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge3StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge2StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge1StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Age3), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Age2), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Age1), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Age3), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Age2), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Age1), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Leader), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Wonder), 3);
	}

}
