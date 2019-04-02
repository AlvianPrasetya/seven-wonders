using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformActionResolver : IResolvable {

	private Player player;

	public PerformActionResolver(Player player = null) {
		this.player = player;
	}

	public IEnumerator Resolve() {
		if (player != null) {
			yield return GameManager.Instance.StartCoroutine(player.PerformAction());
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
