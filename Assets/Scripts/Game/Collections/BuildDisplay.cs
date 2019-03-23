using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDisplay : MonoBehaviour, IPushable<Card> {

	[System.Serializable]
	public class DisplayPileEntry {

		public DisplayType displayType;
		public CardPile cardPile;

	}

	public DisplayPileEntry[] displayPiles;
	public Dictionary<DisplayType, CardPile> DisplayPiles { get; private set; }

	void Awake() {
		DisplayPiles = new Dictionary<DisplayType, CardPile>();
		foreach (DisplayPileEntry displayPile in displayPiles) {
			DisplayPiles.Add(displayPile.displayType, displayPile.cardPile);
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
