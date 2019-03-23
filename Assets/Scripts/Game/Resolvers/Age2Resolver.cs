using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age2Resolver : IResolvable {

	private const int TurnCount = 6;

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new UnloadDeckResolver(DeckType.Age2, Direction.East), 3);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), 3);
		GameManager.Instance.EnqueueResolver(new DecideActionResolver(30), 3);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), 3);
		GameManager.Instance.EnqueueResolver(new PerformActionResolver(Direction.East), 3);
		for (int i = 0; i < TurnCount - 1; i++) {
			GameManager.Instance.EnqueueResolver(new UnloadHandResolver(DeckType.WestDeck, Direction.East), 3);
			GameManager.Instance.EnqueueResolver(new UnloadDeckResolver(DeckType.WestDeck, Direction.East), 3);
			GameManager.Instance.EnqueueResolver(new SyncResolver(), 3);
			GameManager.Instance.EnqueueResolver(new DecideActionResolver(30), 3);
			GameManager.Instance.EnqueueResolver(new SyncResolver(), 3);
			GameManager.Instance.EnqueueResolver(new PerformActionResolver(Direction.East), 3);
		}
		GameManager.Instance.EnqueueResolver(new UnloadHandResolver(DeckType.Discard, Direction.East), 3);

		yield return null;
	}

}
