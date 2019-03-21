using UnityEngine;
using UnityEngine.Events;

public class BuildPlayArea : PlayArea {

	[System.Serializable]
	public class PlayBuildEvent : UnityEvent<Card> {}

	public PlayBuildEvent playEvent;

	public override void Play(Card card) {
		playEvent.Invoke(card);
	}

}
