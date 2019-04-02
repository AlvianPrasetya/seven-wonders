using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age2Resolver : IResolvable {

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.Age2, DeckType.West, Direction.East),
			Priority.Turn
		);
		for (int i = 0; i < GameOptions.TurnsPerAge - 2; i++) {
			GameManager.Instance.EnqueueResolver(
				new TurnResolver(DeckType.West, DeckType.West, Direction.East),
				Priority.Turn
			);
		}
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.West, DeckType.Discard, Direction.East),
			Priority.Turn
		);

		yield return null;
	}

}
