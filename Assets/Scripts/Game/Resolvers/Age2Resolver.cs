using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age2Resolver : IResolvable {

	private const int TurnCount = 6;
	private const int Priority = 4;

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.Age2, DeckType.West, Direction.East),
			Priority
		);
		for (int i = 0; i < TurnCount - 2; i++) {
			GameManager.Instance.EnqueueResolver(
				new TurnResolver(DeckType.West, DeckType.West, Direction.East),
				Priority
			);
		}
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.West, DeckType.Discard, Direction.East),
			Priority
		);

		yield return null;
	}

}
