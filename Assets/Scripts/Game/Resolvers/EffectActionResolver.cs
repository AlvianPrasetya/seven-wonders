using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActionResolver : IResolvable {

	public IEnumerator Resolve() {
		foreach (Player player in GameManager.Instance.Players) {
			player.EffectAction();
		}
		yield return null;
	}

}
