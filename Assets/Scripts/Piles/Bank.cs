using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bank represents piles of coins that supplies all players with coins.
public class Bank : Pile<Coin>, ILoadable {

	public Coin coinPrefab;
	public int initialCoinCount;
	public CoinPile[] coinPiles;
	public override int Count {
		get {
			int count = 0;
			foreach (CoinPile coinPile in coinPiles) {
				count += coinPile.Count;
			}
			return count;
		}
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
	}

	public override Coin Pop() {
		if (Count == 0) {
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

		return tallestCoinPile.Pop();
	}

	public IEnumerator Load() {
		for (int i = 0; i < initialCoinCount; i++) {
			Coin coin = Instantiate(coinPrefab, transform.position, transform.rotation);
			yield return Push(coin);
		}
	}

}
