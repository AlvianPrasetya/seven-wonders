using System;
using System.Collections;
using UnityEngine;

public class GainPointsPerCountResolver : IResolvable {

	public delegate int Count();

	private Player player;
	private PointType pointType;
	private int amountPerCount;
	private Count count;

	public GainPointsPerCountResolver(Player player, PointType pointType, int amountPerCount, Count count) {
		this.player = player;
		this.pointType = pointType;
		this.amountPerCount = amountPerCount;
		this.count = count;
	}

	public IEnumerator Resolve() {
		int totalCount = 0;
		foreach (Count count in count.GetInvocationList()) {
			totalCount += count.Invoke();
		}
		
		yield return player.GainPoints(pointType, amountPerCount * totalCount);
	}

}
