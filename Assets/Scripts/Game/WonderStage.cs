using System.Collections;
using UnityEngine;

public class WonderStage : MonoBehaviour {

	public CardSlot buildCardSlot;

	public IEnumerator Build(Card card) {
		yield return buildCardSlot.Push(card);
	}
	
}
