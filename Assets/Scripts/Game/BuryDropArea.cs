using UnityEngine;
using UnityEngine.Events;

public class BuryDropArea : DropArea<Card> {

	[System.Serializable]
	public class OnDropEvent : UnityEvent<Card, int> {}

	public OnDropEvent onDropEvent;
	public int wonderStage;

	public override void Drop(Card card) {
		onDropEvent.Invoke(card, wonderStage);
	}

}