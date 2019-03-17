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
		Coroutine shuffleAndDealWonder = GameManager.Instance.StartCoroutine(
			ShuffleAndDeal(StockType.Wonder, DeckType.Wonder)
		);

		yield return GameManager.Instance.Stocks[StockType.Age1].Load();
		Coroutine shuffleAndDealAge1 = GameManager.Instance.StartCoroutine(
			ShuffleAndDeal(StockType.Age1, DeckType.Age1)
		);
		yield return GameManager.Instance.Stocks[StockType.Age2].Load();
		Coroutine shuffleAndDealAge2 = GameManager.Instance.StartCoroutine(
			ShuffleAndDeal(StockType.Age2, DeckType.Age2)
		);
		yield return shuffleGuildStock;
		yield return GameManager.Instance.Stocks[StockType.Age3].Load();
		Coroutine shuffleAndDealAge3 = GameManager.Instance.StartCoroutine(
			ShuffleAndDeal(StockType.Age3, DeckType.Age3)
		);
		
		yield return loadBank;
		yield return shuffleAndDealWonder;
		yield return shuffleAndDealAge1;
		yield return shuffleAndDealAge2;
		yield return shuffleAndDealAge3;
	}

	private IEnumerator ShuffleAndDeal(StockType stockType, DeckType deckType) {
		yield return GameManager.Instance.Stocks[stockType].Shuffle(5);
		yield return GameManager.Instance.Stocks[stockType].Deal(deckType);
	}

}
