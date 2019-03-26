using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformActionResolver : IResolvable {

	public IEnumerator Resolve() {
		Queue<Coroutine> performActions = new Queue<Coroutine>();
		foreach (Player player in GameManager.Instance.Players) {
			performActions.Enqueue(GameManager.Instance.StartCoroutine(player.PerformAction()));
		}
		while (performActions.Count != 0) {
			yield return performActions.Dequeue();
		}
	}

}
