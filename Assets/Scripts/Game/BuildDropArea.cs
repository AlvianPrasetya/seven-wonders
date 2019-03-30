using UnityEngine;
using UnityEngine.Events;

public class BuildDropArea : DropArea<Card> {

	[System.Serializable]
	public class OnDropEvent : UnityEvent<Card, Payment> {}

	public OnDropEvent onDropEvent;
	public Payment payment;

	public override void Drop(Card card) {
		onDropEvent.Invoke(card, payment);
	}

}
