using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age3Resolver : IResolvable {

	private const int TurnCount = 6;
	private const int Priority = 4;

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.Age3, DeckType.East, Direction.West),
			Priority
		);
		for (int i = 0; i < TurnCount - 2; i++) {
			GameManager.Instance.EnqueueResolver(
				new TurnResolver(DeckType.East, DeckType.East, Direction.West),
				Priority
			);
		}
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.East, DeckType.Discard, Direction.West),
			Priority
		);

		yield return null;
	}

}
