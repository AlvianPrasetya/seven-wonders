using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformActionResolver : IResolvable {

	private Player[] targetPlayers;

	public PerformActionResolver() {
		targetPlayers = null;
	}

	public PerformActionResolver(params Player[] targetPlayers) {
		this.targetPlayers = targetPlayers;
	}

	public IEnumerator Resolve() {
		if (targetPlayers != null) {
			foreach (Player targetPlayer in targetPlayers) {
				yield return GameManager.Instance.StartCoroutine(targetPlayer.PerformAction());
			}
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
