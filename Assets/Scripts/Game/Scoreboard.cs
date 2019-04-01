using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

	public PlayerScore playerScorePrefab;
	public Vector2 initialSpacing = new Vector2(0, -100);
	public Vector2 spacing = new Vector2(0, -80);

	private Dictionary<Player, PlayerScore> playerScores;

	void Awake() {
		playerScores = new Dictionary<Player, PlayerScore>();
	}

	public bool IsDisplayed {
		set {
			gameObject.SetActive(value);
		}
	}

	public IEnumerator AddPlayer(Player player) {
		PlayerScore playerScore = Instantiate(
			playerScorePrefab, transform.position, transform.rotation, transform
		);
		playerScore.AnchoredPosition = initialSpacing;
		
		playerScore.Nickname = player.Nickname;
		playerScores.Add(player, playerScore);

		yield return MoveToPosition(playerScore, playerScores.Count - 1);
	}

	public IEnumerator AddPoints(Player player, PointType pointType, int amount) {
		yield return playerScores[player].AddPoints(pointType, amount);
	}

	private IEnumerator MoveToPosition(PlayerScore playerScore, int position) {
		Vector2 anchoredPosition = initialSpacing + spacing * position;
		yield return playerScore.MoveToPosition(anchoredPosition);
	}

}
