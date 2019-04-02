using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActionResolver : IResolvable {

	private Player player;

	public EffectActionResolver(Player player = null) {
		this.player = player;
	}

	public IEnumerator Resolve() {
		if (player != null) {
			player.EffectAction();
			yield break;
		}

		foreach (Player player in GameManager.Instance.Players) {
			player.EffectAction();
		}
	}

}
