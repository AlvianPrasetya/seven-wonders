using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

	public PlayerScore playerScorePrefab;
	public Vector2 initialSpacing = new Vector2(0, -100);
	public Vector2 spacing = new Vector2(0, -80);

	private Dictionary<Player, PlayerScore> playerScoresByPlayer;
	private List<PlayerScore> playerScores;

	void Awake() {
		playerScoresByPlayer = new Dictionary<Player, PlayerScore>();
		playerScores = new List<PlayerScore>();
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
		playerScoresByPlayer.Add(player, playerScore);
		playerScores.Add(playerScore);

		yield return playerScore.MoveToPosition(GetAnchoredPosition(playerScoresByPlayer.Count - 1));
	}

	public IEnumerator AddPoints(Player player, PointType pointType, int amount) {
		Coroutine addPoints = StartCoroutine(playerScoresByPlayer[player].AddPoints(pointType, amount));

		// Re-sort player scores
		playerScores.Sort();

		// Rearrange scoreboard entries
		Queue<Coroutine> moveToPositions = new Queue<Coroutine>();
		for (int i = 0; i < playerScores.Count; i++) {
			moveToPositions.Enqueue(StartCoroutine(
				playerScores[i].MoveToPosition(GetAnchoredPosition(i))
			));
		}
		while (moveToPositions.Count != 0) {
			yield return moveToPositions.Dequeue();
		}

		yield return addPoints;
	}

	private Vector3 GetAnchoredPosition(int position) {
		Vector2 anchoredPosition = initialSpacing + spacing * position;
		return anchoredPosition;
	}

}
