using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformActionResolver : IResolvable {

	private Player targetPlayer;

	public PerformActionResolver(Player targetPlayer = null) {
		this.targetPlayer = targetPlayer;
	}

	public IEnumerator Resolve() {
		if (targetPlayer != null) {
			yield return GameManager.Instance.StartCoroutine(targetPlayer.PerformAction());
			yield break;
		}

		Queue<Coroutine> performActions = new Queue<Coroutine>();
		foreach (Player player in GameManager.Instance.Players) {
			performActions.Enqueue(GameManager.Instance.StartCoroutine(player.PerformAction()));
		}
		while (performActions.Count != 0) {
			yield return performActions.Dequeue();
		}
	}

}
