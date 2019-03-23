using System.Collections;
using UnityEngine;

public class PerformActionResolver : IResolvable {

	private Direction performDirection;

	public PerformActionResolver(Direction performDirection) {
		this.performDirection = performDirection;
	}

	public IEnumerator Resolve() {
		switch (performDirection) {
			case Direction.West:
				for (int i = GameManager.Instance.Players.Count - 1; i >= 0; i--) {
					yield return ResolveForPlayer(GameManager.Instance.Players[i]);
				}
				break;
			case Direction.East:
				for (int i = 0; i < GameManager.Instance.Players.Count; i++) {
					yield return ResolveForPlayer(GameManager.Instance.Players[i]);
				}
				break;
		}
	}

	public IEnumerator ResolveForPlayer(Player player) {
		yield return GameManager.Instance.gameCamera.Focus(player);
		yield return player.PerformAction();
		yield return new WaitForSeconds(1);
	}

}
