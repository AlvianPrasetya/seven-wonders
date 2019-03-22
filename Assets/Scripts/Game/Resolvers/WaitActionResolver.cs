using System.Collections;
using UnityEngine;

public class WaitActionResolver : IResolvable {

	public IEnumerator Resolve() {
		bool done = false;
		while (!done) {
			done = true;
			foreach (Player player in GameManager.Instance.Players) {
				if (player.Action == null) {
					done = false;
					break;
				}
			}

			yield return null;
		}
	}

}
