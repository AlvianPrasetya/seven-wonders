using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

	[System.Serializable]
	public class PointEntry {

		public PointType pointType;
		public Text pointText;
		public int Point { get; private set; }

		public void AddPoint(int amount) {
			Point += amount;
			pointText.text = Point.ToString();
		}

	}

	[System.Serializable]
	public class TotalEntry {

		public Text totalText;
		public int Total { get; private set; }

		public void AddPoint(int amount) {
			Total += amount;
			totalText.text = Total.ToString();
		}

	}

	public Text nicknameText;
	public PointEntry[] pointEntries;
	public TotalEntry totalEntry;

	private Dictionary<PointType, PointEntry> pointEntriesByType;

	void Awake() {
		pointEntriesByType = new Dictionary<PointType, PointEntry>();
		foreach (PointEntry pointEntry in pointEntries) {
			pointEntriesByType[pointEntry.pointType] = pointEntry;
		}
	}

	public string Nickname {
		set {
			nicknameText.text = value;
		}
	}

	public int TotalScore {
		get {
			return totalEntry.Total;
		}
	}

	public void AddPoint(PointType pointType, int amount) {
		pointEntriesByType[pointType].AddPoint(amount);
		totalEntry.AddPoint(amount);
	}

}
