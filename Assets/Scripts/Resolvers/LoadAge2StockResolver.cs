using UnityEngine;

public class LoadAge2StockResolver : IResolver {

	public bool IsResolvable() {
		return GameManager.Instance.StockPiles.ContainsKey(StockType.RawMaterial) &&
			GameManager.Instance.StockPiles.ContainsKey(StockType.ManufacturedGoods) &&
			GameManager.Instance.StockPiles.ContainsKey(StockType.Civilian) &&
			GameManager.Instance.StockPiles.ContainsKey(StockType.Scientific) &&
			GameManager.Instance.StockPiles.ContainsKey(StockType.Commercial) &&
			GameManager.Instance.StockPiles.ContainsKey(StockType.Military) &&
			GameManager.Instance.StockPiles.ContainsKey(StockType.City);
	}

	public void Resolve() {
	}

}
