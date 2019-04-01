using System.Collections;
using UnityEngine;

public class PostGameResolver : IResolvable {

	public IEnumerator Resolve() {
		UIManager.Instance.scoreboard.IsDisplayed = true;

		// Add players to scoreboard
		foreach (Player player in GameManager.Instance.Players) {
			yield return UIManager.Instance.scoreboard.AddPlayer(player);
		}

		yield return null;
	}

}
