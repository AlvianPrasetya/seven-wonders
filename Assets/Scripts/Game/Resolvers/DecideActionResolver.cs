using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DecideActionResolver : IResolvable {

	private float decideTime;

	public DecideActionResolver(float decideTime) {
		this.decideTime = decideTime;
	}

	public IEnumerator Resolve() {
		GameManager.Instance.Player.IsActive = true;

		float remainingTime = decideTime;
		while (remainingTime > 0 && GameManager.Instance.Player.Action == null) {
			remainingTime -= Time.deltaTime;
			yield return null;
		}
		
		GameManager.Instance.Player.IsActive = false;
		if (GameManager.Instance.Player.Action == null) {
			// Player has yet to decide on an action, discard a random card
			GameManager.Instance.Player.DecideDiscard(GameManager.Instance.Player.hand.GetRandom());
		}

		if (PhotonNetwork.IsMasterClient) {
			// Master client determines the bots' moves (TODO: Simple AI)
			foreach (Bot bot in GameManager.Instance.Bots) {
				bot.DecideDiscard(bot.hand.GetRandom());
			}
		}

		yield return new WaitForSeconds(1);
	}

}
