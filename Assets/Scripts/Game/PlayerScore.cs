using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

	private const float moveDuration = 1;

	[System.Serializable]
	public class PointEntry {

		private const float morphDuration = 0.5f;

		public PointType pointType;
		public Text pointText;
		public int Points { get; private set; }

		private int displayedPoints;

		public IEnumerator AddPoints(int amount) {
			Points += amount;
			yield return Morph();
		}

		private IEnumerator Morph() {
			int initialDisplayedPoints = displayedPoints;
			float progress = 0;
			while (progress < 1) {
				progress = Mathf.Min(progress + Time.deltaTime / morphDuration, 1);
				DisplayedPoints = (int)Mathf.Lerp(initialDisplayedPoints, Points, progress);
				
				yield return null;
			}
		}

		private int DisplayedPoints {
			set {
				displayedPoints = value;
				pointText.text = value.ToString();
			}
		}

	}

	[System.Serializable]
	public class TotalEntry {

		private const float morphDuration = 0.5f;

		public Text totalText;
		public int Total { get; private set; }

		private int displayedTotal;

		public IEnumerator AddPoints(int amount) {
			Total += amount;
			yield return Morph();
		}

		private IEnumerator Morph() {
			int initialDisplayedTotal = displayedTotal;
			float progress = 0;
			while (progress < 1) {
				progress = Mathf.Min(progress + Time.deltaTime / morphDuration, 1);
				DisplayedTotal = (int)Mathf.Lerp(initialDisplayedTotal, Total, progress);
				
				yield return null;
			}
		}

		private int DisplayedTotal {
			set {
				displayedTotal = value;
				totalText.text = value.ToString();
			}
		}

	}

	public Text nicknameText;
	public PointEntry[] pointEntries;
	public TotalEntry totalEntry;

	private RectTransform rectTransform;
	private Dictionary<PointType, PointEntry> pointEntriesByType;

	void Awake() {
		rectTransform = GetComponent<RectTransform>();

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

	public IEnumerator AddPoints(PointType pointType, int amount) {
		Coroutine addPoints = StartCoroutine(pointEntriesByType[pointType].AddPoints(amount));
		Coroutine addTotal = StartCoroutine(totalEntry.AddPoints(amount));

		yield return addPoints;
		yield return addTotal;
	}

	public Vector2 AnchoredPosition {
		set {
			rectTransform.anchoredPosition = value;
		}
	}
	
	public IEnumerator MoveToPosition(Vector2 targetAnchoredPosition) {
		Vector2 initialAnchoredPosition = rectTransform.anchoredPosition;
		float progress = 0;
		while (progress < 1) {
			progress = Mathf.Min(progress + Time.deltaTime / moveDuration, 1);
			Vector2 anchoredPosition = Vector2.Lerp(initialAnchoredPosition, targetAnchoredPosition, progress);
			rectTransform.anchoredPosition = anchoredPosition;

			yield return null;
		}
	}

}
