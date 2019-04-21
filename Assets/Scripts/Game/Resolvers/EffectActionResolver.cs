using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActionResolver : IResolvable {

	private Player[] targetPlayers;

	public EffectActionResolver() {
		targetPlayers = null;
	}

	public EffectActionResolver(params Player[] targetPlayers) {
		this.targetPlayers = targetPlayers;
	}

	public IEnumerator Resolve() {
		if (targetPlayers != null) {
			foreach (Player targetPlayer in targetPlayers) {
				targetPlayer.EffectAction();
			}
			yield break;
		}

		foreach (Player player in GameManager.Instance.Players) {
			player.EffectAction();
		}
	}

}
