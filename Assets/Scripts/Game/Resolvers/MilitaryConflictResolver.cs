using System.Collections;
using UnityEngine;

public class MilitaryConflictResolver : IResolvable {

	public MilitaryToken victoryTokenPrefab;
	public MilitaryToken drawTokenPrefab;
	public MilitaryToken defeatTokenPrefab;

	public MilitaryConflictResolver(
		MilitaryToken victoryTokenPrefab, MilitaryToken drawTokenPrefab, MilitaryToken defeatTokenPrefab
	) {
		this.victoryTokenPrefab = victoryTokenPrefab;
		this.drawTokenPrefab = drawTokenPrefab;
		this.defeatTokenPrefab = defeatTokenPrefab;
	}

	public IEnumerator Resolve() {
		foreach (Player player in GameManager.Instance.Players) {
			if (player.IsPeaceful) {
				// Peace is treated as draws to both sides
				yield return player.GainMilitaryToken(drawTokenPrefab);
				yield return player.GainMilitaryToken(drawTokenPrefab);
				continue;
			}

			// Evaluate west opponent (skip peaceful players)
			Player westOpponent = player.Neighbours[Direction.West];
			while (westOpponent.IsPeaceful) {
				westOpponent = westOpponent.Neighbours[Direction.West];
			}
			// Evaluate east opponent (skip peaceful players)
			Player eastOpponent = player.Neighbours[Direction.East];
			while (eastOpponent.IsPeaceful) {
				eastOpponent = eastOpponent.Neighbours[Direction.East];
			}

			int shields = player.ShieldCount;
			int westShields = westOpponent.ShieldCount;
			int eastShields = eastOpponent.ShieldCount;
			
			MilitaryToken westMilitaryToken;
			if (shields > westShields) {
				westMilitaryToken = GameObject.Instantiate(victoryTokenPrefab, Vector3.zero, Quaternion.identity);
			} else if (shields == westShields) {
				westMilitaryToken = GameObject.Instantiate(drawTokenPrefab, Vector3.zero, Quaternion.identity);
			} else {
				westMilitaryToken = GameObject.Instantiate(defeatTokenPrefab, Vector3.zero, Quaternion.identity);
			}
			yield return player.GainMilitaryToken(westMilitaryToken);

			MilitaryToken eastMilitaryToken;
			if (shields > eastShields) {
				eastMilitaryToken = GameObject.Instantiate(victoryTokenPrefab, Vector3.zero, Quaternion.identity);
			} else if (shields == eastShields) {
				eastMilitaryToken = GameObject.Instantiate(drawTokenPrefab, Vector3.zero, Quaternion.identity);
			} else {
				eastMilitaryToken = GameObject.Instantiate(defeatTokenPrefab, Vector3.zero, Quaternion.identity);
			}
			yield return player.GainMilitaryToken(eastMilitaryToken);
		}

		// Reset peaceful status for all players
		foreach (Player player in GameManager.Instance.Players) {
			player.IsPeaceful = false;
		}
	}

}
