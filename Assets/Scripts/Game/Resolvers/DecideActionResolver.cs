using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DecideActionResolver : IResolvable {

	private float decideTime;

	public DecideActionResolver(float decideTime) {
		this.decideTime = decideTime;
	}

	public IEnumerator Resolve() {
		GameManager.Instance.Player.IsPlayable = true;
		
		UIManager.Instance.StartTimer(decideTime);
		float remainingTime = decideTime;
		while (remainingTime > 0 && GameManager.Instance.Player.Action == null) {
			remainingTime -= Time.deltaTime;
			yield return null;
		}
		UIManager.Instance.StopTimer();
		
		GameManager.Instance.Player.IsPlayable = false;

		if (GameManager.Instance.Player.Action == null) {
			// Player has yet to decide on an action, discard a random card
			GameManager.Instance.Player.DecideDiscard(GameManager.Instance.Player.hand.GetRandom());
		}
	}

}
