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
		if (PhotonNetwork.IsMasterClient) {
			// Master client triggers the bot decisions
			foreach (Bot bot in GameManager.Instance.Bots) {
				bot.IsPlayable = true;
				bot.IsPlayable = false;
			}
		}

		UIManager.Instance.StartTimer(decideTime);
		float remainingTime = decideTime;
		while (remainingTime > 0 && GameManager.Instance.Player.Action == null) {
			remainingTime -= Time.deltaTime;
			yield return null;
		}
		UIManager.Instance.StopTimer();
		
		GameManager.Instance.Player.IsPlayable = false;
		yield return new WaitForSeconds(1);
	}

}
