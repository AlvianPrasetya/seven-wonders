using System.Collections.Generic;
using UnityEngine;

public class CardPile : MonoBehaviour {

	public List<Card> cards;

	void Awake() {
		cards = new List<Card>();
	}

}
