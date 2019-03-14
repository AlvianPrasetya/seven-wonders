using UnityEngine;

public class LoadAge1StockResolver : IResolver {

    public bool IsResolvable() {
		return GameManager.Instance.StockPiles.ContainsKey(Stock.RawMaterials) &&
			GameManager.Instance.StockPiles.ContainsKey(Stock.ManufacturedGoods) &&
			GameManager.Instance.StockPiles.ContainsKey(Stock.Civilian) &&
			GameManager.Instance.StockPiles.ContainsKey(Stock.Scientific) &&
			GameManager.Instance.StockPiles.ContainsKey(Stock.Commercial) &&
			GameManager.Instance.StockPiles.ContainsKey(Stock.Military) &&
			GameManager.Instance.StockPiles.ContainsKey(Stock.City);
	}

    public void Resolve() {
    }

}
