using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActionResolver : IResolvable {

	private Player targetPlayer;

	public EffectActionResolver(Player targetPlayer = null) {
		this.targetPlayer = targetPlayer;
	}

	public IEnumerator Resolve() {
		if (targetPlayer != null) {
			targetPlayer.EffectAction();
			yield break;
		}

		foreach (Player player in GameManager.Instance.Players) {
			player.EffectAction();
		}
	}

}
