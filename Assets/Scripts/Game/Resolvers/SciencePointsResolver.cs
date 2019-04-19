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
		List<Science> sciences = player.Sciences;

		if (pos == sciences.Count) {
			return 0;
		}

		if (counts == null) {
			counts = new int[Enum.GetNames(typeof(ScienceType)).Length];
		}

		int maxPoints = 0;
		foreach (ScienceType scienceType in sciences[pos].ScienceTypes) {
			int prevSymbolCount = counts[(int)scienceType];
			int prevSetCount = counts.Min();

			counts[(int)scienceType]++;

			int symbolCount = counts[(int)scienceType];
			int setCount = counts.Min();

			maxPoints = Mathf.Max(
				maxPoints,
				ResolvePoints(pos + 1, counts) +
					symbolCount * symbolCount - prevSymbolCount * prevSymbolCount +
					pointsPerScienceSet * (setCount - prevSetCount)
			);

			counts[(int)scienceType]--;
		}

		return maxPoints;
	}

	public void AddPointsPerScienceSet(int extraPointsPerScienceSet) {
		pointsPerScienceSet += extraPointsPerScienceSet;
	}

}
