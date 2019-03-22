using UnityEngine;
using UnityEngine.Events;

public class BuildDropArea : DropArea<Card> {

	[System.Serializable]
	public class OnDropEvent : UnityEvent<Card> {}

	public OnDropEvent onDropEvent;

	public override void Drop(Card card) {
		onDropEvent.Invoke(card);
	}

}
