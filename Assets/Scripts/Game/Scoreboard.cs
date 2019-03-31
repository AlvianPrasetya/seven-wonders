using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

	public PlayerScore playerScorePrefab;

	private Dictionary<Player, PlayerScore> playerScores;

	void Awake() {
		playerScores = new Dictionary<Player, PlayerScore>();
	}

	public void AddPlayer(Player player) {
		PlayerScore playerScore = Instantiate(
			playerScorePrefab, transform.position, transform.rotation, transform
		);
		playerScore.Nickname = player.Nickname;
		playerScores.Add(player, playerScore);
	}

}
