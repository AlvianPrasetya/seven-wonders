using System.Collections;
using UnityEngine;

public class RevealActionResolver : IResolvable {

	private Direction revealDirection;

	public RevealActionResolver(Direction revealDirection) {
		this.revealDirection = revealDirection;
	}

	public IEnumerator Resolve() {
		switch (revealDirection) {
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

	private IEnumerator ResolveForPlayer(Player player) {
		yield return GameManager.Instance.gameCamera.Focus(player);
		yield return player.preparedCardSlot.Flip();
		yield return new WaitForSeconds(1);
	}

}
