using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CoinPile represents a pile of coin.
public class CoinPile : Pile<Coin> {

	private const float DropSpacing = 0.2f;
	
	public Stack<Coin> Coins { get; protected set; }
	public override int Count {
		get {
			return Coins.Count;
		}
	}

	void Awake() {
		Coins = new Stack<Coin>();
	}

	public override IEnumerator Push(Coin coin) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Coins.Count + 1);
		yield return coin.MoveTowards(dropPosition, transform.rotation, 100, 360);

		Coins.Push(coin);
		coin.transform.parent = transform;
	}

	public override Coin Pop() {
		if (Coins.Count == 0) {
			return null;
		}

		return Coins.Pop();
	}

}
