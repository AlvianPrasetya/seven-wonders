using System;
using System.Collections;
using UnityEngine;

public class PreGameResolver : IResolvable {

	private int randomSeed;

	public PreGameResolver(int randomSeed) {
		this.randomSeed = randomSeed;
	}

	public IEnumerator Resolve() {
		System.Random random = new System.Random(randomSeed);
		int guildRandomSeed = random.Next();
		int age3RandomSeed = random.Next();
		int age2RandomSeed = random.Next();
		int age1RandomSeed = random.Next();

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
		Coroutine loadAndShuffleGuildStock = GameManager.Instance.StartCoroutine(
			LoadAndShuffle(StockType.Guild, 5, guildRandomSeed)
		);

		yield return loadRawMaterialStock;
		yield return loadManufacturedGoodsStock;
		yield return loadCivilianStock;
		yield return loadScientificStock;
		yield return loadCommercialStock;
		yield return loadMilitaryStock;
		yield return loadAndShuffleGuildStock;

		Coroutine moveCameraShuffle = GameManager.Instance.StartCoroutine(
			GameManager.Instance.gameCamera.MoveTowards(new Vector3(0, 40, -20), Quaternion.Euler(75, 0, 0), 10)
		);
		
		yield return GameManager.Instance.Stocks[StockType.Age3].Load();
		Coroutine shuffleAge3 = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age3].Shuffle(5, age3RandomSeed)
		);
		yield return GameManager.Instance.Stocks[StockType.Age2].Load();
		Coroutine shuffleAge2 = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age2].Shuffle(5, age2RandomSeed)
		);
		yield return GameManager.Instance.Stocks[StockType.Age1].Load();
		Coroutine shuffleAge1 = GameManager.Instance.StartCoroutine(
			GameManager.Instance.Stocks[StockType.Age1].Shuffle(5, age1RandomSeed)
		);

		yield return moveCameraShuffle;
		Coroutine moveCameraDeal = GameManager.Instance.StartCoroutine(
			GameManager.Instance.gameCamera.MoveTowards(new Vector3(0, 85, 0), Quaternion.Euler(90, 0, 0), 10)
		);

		yield return shuffleAge3;
		yield return GameManager.Instance.Stocks[StockType.Age3].Deal(DeckType.Age3);
		yield return shuffleAge2;
		yield return GameManager.Instance.Stocks[StockType.Age2].Deal(DeckType.Age2);
		yield return shuffleAge1;
		yield return GameManager.Instance.Stocks[StockType.Age1].Deal(DeckType.Age1);
		
		yield return loadBank;

		yield return moveCameraDeal;

		foreach (Player player in GameManager.Instance.Players) {
			yield return player.GainCoin(GameOptions.InitialCoinAmount);
		}
	}

	private IEnumerator LoadAndShuffle(StockType stockType, int numIterations, int randomSeed) {
		yield return GameManager.Instance.Stocks[stockType].Load();
		yield return GameManager.Instance.Stocks[stockType].Shuffle(numIterations, randomSeed);
	}

}
