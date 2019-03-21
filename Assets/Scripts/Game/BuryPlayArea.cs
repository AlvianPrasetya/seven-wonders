using UnityEngine;
using UnityEngine.Events;

public class BuryPlayArea : PlayArea {

	[System.Serializable]
	public class PlayBuildEvent : UnityEvent<Card, int> {}

	public PlayBuildEvent playEvent;
	public int wonderStage;

	public override void Play(Card card) {
		playEvent.Invoke(card, wonderStage);
	}

}
