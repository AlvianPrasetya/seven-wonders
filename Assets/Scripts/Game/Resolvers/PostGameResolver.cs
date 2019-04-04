using System.Collections;
using UnityEngine;

public class PostGameResolver : IResolvable {

	public IEnumerator Resolve() {
		UIManager.Instance.scoreboard.IsDisplayed = true;

		// Add players to scoreboard
		foreach (Player player in GameManager.Instance.Players) {
			yield return UIManager.Instance.scoreboard.AddPlayer(player);
		}

		// Tally points from treasury
		foreach (Player player in GameManager.Instance.Players) {
			GameManager.Instance.EnqueueResolver(
				new GainPointsResolver(player, PointType.Treasury, () => {
					return player.bank.Count / GameOptions.CoinsPerTreasuryPoint;
				}),
				Priority.GainPoints
			);
		}

		yield return null;
	}

}
