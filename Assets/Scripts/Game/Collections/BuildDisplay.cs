using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDisplay : MonoBehaviour, IPushable<Card> {

	[System.Serializable]
	public class DisplayPileEntry {

		public DisplayType displayType;
		public DisplayPile displayPile;

	}

	public DisplayPileEntry[] displayPileEntries;
	public Dictionary<DisplayType, DisplayPile> DisplayPiles { get; private set; }

	void Awake() {
		DisplayPiles = new Dictionary<DisplayType, DisplayPile>();
		foreach (DisplayPileEntry displayPileEntry in displayPileEntries) {
			DisplayPiles.Add(displayPileEntry.displayType, displayPileEntry.displayPile);
		}
	}

	public IEnumerator Push(Card card) {
		yield return DisplayPiles[card.displayType].Push(card);
	}

	public IEnumerator PushMany(Card[] cards) {
		foreach (Card card in cards) {
			yield return Push(card);
		}
	}

}
