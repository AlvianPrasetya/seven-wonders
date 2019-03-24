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
					GameManager.Instance.EnqueueResolver(
						new PerformPlayerActionResolver(GameManager.Instance.Players[i]),
						4
					);
				}
				break;
			case Direction.East:
				for (int i = 0; i < GameManager.Instance.Players.Count; i++) {
					GameManager.Instance.EnqueueResolver(
						new PerformPlayerActionResolver(GameManager.Instance.Players[i]),
						4
					);
				}
				break;
		}

		yield return null;
	}

}
