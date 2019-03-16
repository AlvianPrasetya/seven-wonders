using System.Collections;
using UnityEngine;

public class PreGameResolver : IResolvable {

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new LoadStockResolver(StockType.RawMaterial), 3);
		GameManager.Instance.EnqueueResolver(new LoadStockResolver(StockType.ManufacturedGoods), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.RawMaterial, 5), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Guild, 5), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.City, 5), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Leader, 5), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Wonder, 5), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge3StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge2StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new LoadAge1StockResolver(), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Age3, 5), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Age2, 5), 3);
		GameManager.Instance.EnqueueResolver(new ShuffleStockResolver(StockType.Age1, 5), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Age3), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Age2), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Age1), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Leader), 3);
		GameManager.Instance.EnqueueResolver(new DealStockResolver(StockType.Wonder), 3);
		
		yield return null;
	}

}
