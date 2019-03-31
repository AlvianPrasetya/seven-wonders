using System;
using System.Collections;
using UnityEngine;

public class GainCoinsPerCountResolver : IResolvable {

	public delegate int Count();

	private Player player;
	private int amountPerCount;
	private Count count;

	public GainCoinsPerCountResolver(Player player, int amountPerCount, Count count) {
		this.player = player;
		this.amountPerCount = amountPerCount;
		this.count = count;
	}

	public IEnumerator Resolve() {
		int totalCount = 0;
		foreach (Count count in count.GetInvocationList()) {
			totalCount += count.Invoke();
		}
		
		yield return player.GainCoins(amountPerCount * totalCount);
	}

}
