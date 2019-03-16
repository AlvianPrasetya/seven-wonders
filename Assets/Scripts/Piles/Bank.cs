using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bank represents piles of coins that supplies all players with coins.
public class Bank : Pile<Coin>, ILoadable {

	public Coin coinPrefab;
	public int initialCoinCount;
	public CoinPile[] coinPiles;

	void Awake() {
		Elements = new LinkedList<Coin>();
	}

	public override IEnumerator Push(Coin coin) {
		CoinPile shortestCoinPile = null;
		foreach (CoinPile coinPile in coinPiles) {
			if (shortestCoinPile == null) {
				shortestCoinPile = coinPile;
				continue;
			}

			if (coinPile.Count < shortestCoinPile.Count) {
				shortestCoinPile = coinPile;
			}
		}

		yield return shortestCoinPile.Push(coin);

		Elements.AddLast(coin);
	}

	public override Coin Pop() {
		if (Elements.Count == 0) {
			return null;
		}

		CoinPile tallestCoinPile = null;
		foreach (CoinPile coinPile in coinPiles) {
			if (tallestCoinPile == null) {
				tallestCoinPile = coinPile;
				continue;
			}

			if (coinPile.Count >= tallestCoinPile.Count) {
				tallestCoinPile = coinPile;
			}
		}

		Coin poppedCoin = tallestCoinPile.Pop();
		Coin topCoin = Elements.Last.Value;
		if (poppedCoin != topCoin) {
			Debug.LogError("Popped coin and top coin mismatched");
		}

		Elements.RemoveLast();
		return topCoin;
	}

	public IEnumerator Load() {
		for (int i = 0; i < initialCoinCount; i++) {
			Coin coin = Instantiate(coinPrefab, transform.position, transform.rotation);
			yield return Push(coin);
		}
	}

}
