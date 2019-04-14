using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SciencePointsResolver : IResolvable {

	private Player player;
	private int pointsPerScienceSet;

	public SciencePointsResolver(Player player) {
		this.player = player;
		this.pointsPerScienceSet = GameOptions.DefaultPointsPerScienceSet;
	}

	public IEnumerator Resolve() {
		yield return new System.NotImplementedException();
	}

	public int ResolvePoints(int pos = 0, int[] counts = null) {
		List<ScienceOptions> sciences = player.Sciences;

		if (pos == sciences.Count) {
			return 0;
		}

		if (counts == null) {
			counts = new int[Enum.GetNames(typeof(Science)).Length];
		}

		int maxPoints = 0;
		foreach (Science science in sciences[pos].Sciences) {
			int prevSymbolCount = counts[(int)science];
			int prevSetCount = counts.Min();

			counts[(int)science]++;

			int symbolCount = counts[(int)science];
			int setCount = counts.Min();

			maxPoints = Mathf.Max(
				maxPoints,
				ResolvePoints(pos + 1, counts) +
					symbolCount * symbolCount - prevSymbolCount * prevSymbolCount +
					pointsPerScienceSet * (setCount - prevSetCount)
			);

			counts[(int)science]--;
		}

		return maxPoints;
	}

	public void AddPointsPerScienceSet(int extraPointsPerScienceSet) {
		pointsPerScienceSet += extraPointsPerScienceSet;
	}

}
