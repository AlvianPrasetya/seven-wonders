using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bank represents evenly distributed piles of coins.
public class Bank : MonoBehaviour, IPushable<Coin>, IPoppable<Coin>, ILoadable {

	public Coin coinPrefab;
	public int initialCoinCount;
	public CoinPile[] coinPiles;
	public int Count {
		get {
			int count = 0;
			foreach (CoinPile coinPile in coinPiles) {
				count += coinPile.Count;
			}
			return count;
		}
	}

	public IEnumerator Push(Coin coin) {
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

	public IEnumerator PushMany(Coin[] coins) {
		foreach (Coin coin in coins) {
			yield return Push(coin);
		}
	}

	public Coin Pop() {
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

	public Coin[] PopMany(int count) {
		List<Coin> poppedCoins = new List<Coin>();
		while (poppedCoins.Count != count && Count != 0) {
			poppedCoins.Add(Pop());
		}

		return poppedCoins.ToArray();
	}

	public IEnumerator Load() {
		Queue<Coroutine> pushCoins = new Queue<Coroutine>();
		for (int i = 0; i < initialCoinCount; i++) {
			Coin coin = Instantiate(coinPrefab, transform.position, transform.rotation);
			pushCoins.Enqueue(StartCoroutine(Push(coin)));

			if (pushCoins.Count == coinPiles.Length) {
				// Deal to all the piles at a time
				while (pushCoins.Count != 0) {
					yield return pushCoins.Dequeue();
				}
			}
		}
		while (pushCoins.Count != 0) {
			yield return pushCoins.Dequeue();
		}
	}

}
