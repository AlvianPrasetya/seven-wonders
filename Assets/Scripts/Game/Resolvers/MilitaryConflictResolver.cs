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
			int shields = player.ShieldCount;
			int westShields = player.Neighbours[Direction.West].ShieldCount;
			int eastShields = player.Neighbours[Direction.East].ShieldCount;
			
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
	}

}
