using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CoinPile represents a pile of coin.
public class CoinPile : Pile<Coin> {

	private const float DropSpacing = 0.2f;

	void Awake() {
		Elements = new LinkedList<Coin>();
	}

	public override IEnumerator Push(Coin coin) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Elements.Count + 1);
		yield return coin.MoveTowards(dropPosition, transform.rotation, 100, 360);

		Elements.AddLast(coin);
	}

	public override Coin Pop() {
		if (Elements.Count == 0) {
			return null;
		}

		Coin topCoin = Elements.Last.Value;
		Elements.RemoveLast();
		return topCoin;
	}

}
