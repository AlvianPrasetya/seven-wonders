using System.Collections;
using UnityEngine;

public class PreGameResolver : IResolvable {

	public IEnumerator Resolve() {
		Coroutine loadBank = GameManager.Instance.StartCoroutine(
			GameManager.Instance.bank.Load()
		);
		Coroutine loadRawMaterialStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.RawMaterial].Load()
		);
		Coroutine loadManufacturedGoodsStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.ManufacturedGoods].Load()
		);
		Coroutine loadCivilianStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Civilian].Load()
		);
		Coroutine loadScientificStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Scientific].Load()
		);
		Coroutine loadCommercialStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Commercial].Load()
		);
		Coroutine loadMilitaryStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Military].Load()
		);
		Coroutine loadGuildStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Guild].Load()
		);
		Coroutine loadWonderStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Wonder].Load()
		);
		yield return loadRawMaterialStock;
		yield return loadManufacturedGoodsStock;
		yield return loadCivilianStock;
		yield return loadScientificStock;
		yield return loadCommercialStock;
		yield return loadMilitaryStock;
		yield return loadGuildStock;
		yield return loadWonderStock;

		Coroutine shuffleGuildStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Guild].Shuffle(5)
		);
		Coroutine shuffleWonderStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Wonder].Shuffle(5)
		);
		yield return shuffleGuildStock;
		yield return shuffleWonderStock;

		Coroutine dealWonderStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Wonder].Deal(GameManager.Instance.players.Length * 2)
		);

		yield return GameManager.Instance.Stocks[StockType.Age1].Load();
		Coroutine shuffleAge1Stock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age1].Shuffle(5)
		);
		yield return GameManager.Instance.Stocks[StockType.Age2].Load();
		Coroutine shuffleAge2Stock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age2].Shuffle(5)
		);
		yield return GameManager.Instance.Stocks[StockType.Age3].Load();
		Coroutine shuffleAge3Stock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age3].Shuffle(5)
		);

		yield return shuffleAge1Stock;
		Coroutine dealAge1Stock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age1].Deal(GameManager.Instance.Stocks[StockType.Age1].Count)
		);
		yield return shuffleAge2Stock;
		Coroutine dealAge2Stock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age2].Deal(GameManager.Instance.Stocks[StockType.Age2].Count)
		);
		yield return shuffleAge3Stock;
		Coroutine dealAge3Stock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age3].Deal(GameManager.Instance.Stocks[StockType.Age3].Count)
		);
		
		yield return loadBank;
		yield return dealWonderStock;
		yield return dealAge1Stock;
		yield return dealAge2Stock;
		yield return dealAge3Stock;
	}

}
