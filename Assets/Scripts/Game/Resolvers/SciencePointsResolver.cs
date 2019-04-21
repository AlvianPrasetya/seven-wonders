using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SciencePointsResolver : IResolvable {

	private struct State {

		public int Pos { get; private set; }
		public int Compass { get; private set; }
		public int Tablet { get; private set; }
		public int Gear { get; private set; }

		public State(int pos, int compass, int tablet, int gear) {
			Pos = pos;
			Compass = compass;
			Tablet = tablet;
			Gear = gear;
		}

		public override bool Equals(object obj) {
			State other = (State)obj;
			return Pos.Equals(other.Pos) && Compass.Equals(other.Compass) &&
				Tablet.Equals(other.Tablet) && Gear.Equals(other.Gear);
		}

		public override int GetHashCode() {
			int hash = 0;
			hash = hash * 23 + Pos.GetHashCode();
			hash = hash * 23 + Compass.GetHashCode();
			hash = hash * 23 + Tablet.GetHashCode();
			hash = hash * 23 + Gear.GetHashCode();
			return hash;
		}

	}

	private Player player;
	private int pointsPerScienceSet;
	// Statically allocated processing variables to optimize memory usage
	private List<Science> sciences;
	private Multiset<ScienceType> neighbourScienceTypes;
	private Dictionary<State, int> memo;

	public SciencePointsResolver(Player player) {
		this.player = player;
		pointsPerScienceSet = GameOptions.DefaultPointsPerScienceSet;
		sciences = new List<Science>();
		neighbourScienceTypes = new Multiset<ScienceType>();
		memo = new Dictionary<State, int>();
	}

	public IEnumerator Resolve() {
		yield return new System.NotImplementedException();
	}

	public int ResolvePoints() {
		EvaluateSciences();
		memo.Clear();

		return CalculateMaxPoints();
	}

	public void AddPointsPerScienceSet(int extraPointsPerScienceSet) {
		pointsPerScienceSet += extraPointsPerScienceSet;
	}

	private void EvaluateSciences() {
		sciences.Clear();
		neighbourScienceTypes.Clear();

		foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
			foreach (Science science in player.Neighbours[direction].ProducedSciences) {
				neighbourScienceTypes.Add(science.ScienceTypes);
			}
		}

		int numCopy = 0;
		foreach (Science science in player.Sciences) {
			if (science.ScienceTypes.Length == 1 && science.ScienceTypes[0] == ScienceType.Copy) {
				numCopy++;
				continue;
			}

			sciences.Add(science);
		}

		for (int i = 0; i < numCopy; i++) {
			List<ScienceType> copiableScienceTypes = new List<ScienceType>();
			if (neighbourScienceTypes.Contains(ScienceType.Compass)) {
				copiableScienceTypes.Add(ScienceType.Compass);
				neighbourScienceTypes.Remove(ScienceType.Compass);
			}
			if (neighbourScienceTypes.Contains(ScienceType.Tablet)) {
				copiableScienceTypes.Add(ScienceType.Tablet);
				neighbourScienceTypes.Remove(ScienceType.Tablet);
			}
			if (neighbourScienceTypes.Contains(ScienceType.Gear)) {
				copiableScienceTypes.Add(ScienceType.Gear);
				neighbourScienceTypes.Remove(ScienceType.Gear);
			}
			
			sciences.Add(new Science(false, copiableScienceTypes.ToArray()));
		}
	}

	private int CalculateMaxPoints(int pos = 0, int compass = 0, int tablet = 0, int gear = 0) {
		if (pos == sciences.Count) {
			return 0;
		}

		int memoizedMaxPoints;
		if (memo.TryGetValue(new State(pos, compass, tablet, gear), out memoizedMaxPoints)) {
			return memoizedMaxPoints;
		}

		int maxPoints = 0;
		int set = Min(compass, tablet, gear);

		if (sciences[pos].ScienceTypes.Contains(ScienceType.Compass)) {
			int newCompass = compass + 1;
			int newSet = Min(newCompass, tablet, gear);
			maxPoints = Mathf.Max(
				maxPoints,
				CalculateMaxPoints(pos + 1, newCompass, tablet, gear) +
					newCompass * newCompass - compass * compass +
					pointsPerScienceSet * (newSet - set)
			);
		}

		if (sciences[pos].ScienceTypes.Contains(ScienceType.Tablet)) {
			int newTablet = tablet + 1;
			int newSet = Min(compass, newTablet, gear);
			maxPoints = Mathf.Max(
				maxPoints,
				CalculateMaxPoints(pos + 1, compass, newTablet, gear) +
					newTablet * newTablet - tablet * tablet +
					pointsPerScienceSet * (newSet - set)
			);
		}

		if (sciences[pos].ScienceTypes.Contains(ScienceType.Gear)) {
			int newGear = gear + 1;
			int newSet = Min(compass, tablet, newGear);
			maxPoints = Mathf.Max(
				maxPoints,
				CalculateMaxPoints(pos + 1, compass, tablet, newGear) +
					newGear * newGear - gear * gear +
					pointsPerScienceSet * (newSet - set)
			);
		}
		
		return memo[new State(pos, compass, tablet, gear)] = maxPoints;
	}

	private int Min(params int[] nums) {
		return nums.Min();
	}

}
