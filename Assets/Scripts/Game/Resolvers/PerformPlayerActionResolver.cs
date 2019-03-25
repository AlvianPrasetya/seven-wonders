using System.Collections;
using UnityEngine;

public class PerformPlayerActionResolver : IResolvable {

	private Player player;

	public PerformPlayerActionResolver(Player player) {
		this.player = player;
	}

	public IEnumerator Resolve() {
		yield return GameManager.Instance.gameCamera.Focus(player);
		yield return player.PerformAction();
	}

}
