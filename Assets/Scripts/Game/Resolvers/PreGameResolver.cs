using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameResolver : IResolvable {

	private int randomSeed;

	public PreGameResolver(int randomSeed) {
		this.randomSeed = randomSeed;
	}

	public IEnumerator Resolve() {
		System.Random random = new System.Random(randomSeed);
		int wonderRandomSeed = random.Next();
		int guildRandomSeed = random.Next();
		int cityRandomSeed = random.Next();
		int leaderRandomSeed = random.Next();
		int age3RandomSeed = random.Next();
		int age2RandomSeed = random.Next();
		int age1RandomSeed = random.Next();

		yield return GameManager.Instance.wonderStock.RandomLoad(wonderRandomSeed);
		yield return GameManager.Instance.wonderStock.Shuffle(5, wonderRandomSeed);
		yield return GameManager.Instance.wonderStock.Deal();
		yield return GameManager.Instance.wonderStock.Dump();

		Coroutine loadBank = GameManager.Instance.StartCoroutine(
			GameManager.Instance.bank.Load()
		);
		Coroutine loadRawMaterialStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.RawMaterial].Load()
		);
		Coroutine loadManufacturedGoodsStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.ManufacturedGoods].Load()
		);
		Coroutine loadCivilianStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.Civilian].Load()
		);
		Coroutine loadScientificStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.Scientific].Load()
		);
		Coroutine loadCommercialStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.Commercial].Load()
		);
		Coroutine loadMilitaryStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.Military].Load()
		);
		Coroutine loadCityStock = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.City].RandomLoad(cityRandomSeed)
		);
		Coroutine loadAndShuffleGuildStock = GameManager.Instance.StartCoroutine(
			LoadAndShuffle(StockType.Guild, 4, guildRandomSeed)
		);
		Coroutine loadShuffleDealLeaderStock = GameManager.Instance.StartCoroutine(
			LoadShuffleDeal(StockType.Leader, 5, leaderRandomSeed)
		);

		yield return loadRawMaterialStock;
		yield return loadManufacturedGoodsStock;
		yield return loadCivilianStock;
		yield return loadScientificStock;
		yield return loadCommercialStock;
		yield return loadMilitaryStock;
		yield return loadCityStock;
		yield return loadAndShuffleGuildStock;
		
		yield return GameManager.Instance.CardStocks[StockType.Age3].Load();
		Coroutine shuffleAge3 = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.Age3].Shuffle(5, age3RandomSeed)
		);
		yield return GameManager.Instance.CardStocks[StockType.Age2].Load();
		Coroutine shuffleAge2 = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.Age2].Shuffle(5, age2RandomSeed)
		);
		yield return GameManager.Instance.CardStocks[StockType.Age1].Load();
		Coroutine shuffleAge1 = GameManager.Instance.StartCoroutine(
			GameManager.Instance.CardStocks[StockType.Age1].Shuffle(5, age1RandomSeed)
		);
		
		yield return GameManager.Instance.CardStocks[StockType.Guild].Dump();

		yield return loadShuffleDealLeaderStock;
		yield return shuffleAge3;
		yield return GameManager.Instance.CardStocks[StockType.Age3].Deal();
		yield return shuffleAge2;
		yield return GameManager.Instance.CardStocks[StockType.Age2].Deal();
		yield return shuffleAge1;
		yield return GameManager.Instance.CardStocks[StockType.Age1].Deal();
		
		yield return loadBank;
		Queue<Coroutine> gainCoins = new Queue<Coroutine>();
		foreach (Player player in GameManager.Instance.Players) {
			gainCoins.Enqueue(GameManager.Instance.StartCoroutine(
				player.GainCoins(GameOptions.InitialCoinAmount)
			));
		}
		while (gainCoins.Count != 0) {
			yield return gainCoins.Dequeue();
		}

		yield return GameManager.Instance.gameCamera.Focus(GameManager.Instance.Player);
	}

	private IEnumerator LoadAndShuffle(StockType stockType, int numIterations, int randomSeed) {
		yield return GameManager.Instance.CardStocks[stockType].Load();
		yield return GameManager.Instance.CardStocks[stockType].Shuffle(numIterations, randomSeed);
	}

	private IEnumerator LoadShuffleDeal(StockType stockType, int numIterations, int randomSeed) {
		yield return LoadAndShuffle(stockType, numIterations, randomSeed);
		yield return GameManager.Instance.CardStocks[stockType].Deal();
	}

}
