using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age3Resolver : IResolvable {

	private const int TurnCount = 6;

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new UnloadDeckResolver(DeckType.Age3, Direction.West), 3);
		GameManager.Instance.EnqueueResolver(new DecideActionResolver(30), 3);
		GameManager.Instance.EnqueueResolver(new WaitActionResolver(), 3);
		GameManager.Instance.EnqueueResolver(new RevealActionResolver(Direction.West), 3);
		GameManager.Instance.EnqueueResolver(new PerformActionResolver(Direction.West), 3);
		for (int i = 0; i < TurnCount - 1; i++) {
			GameManager.Instance.EnqueueResolver(new UnloadHandResolver(DeckType.EastDeck, Direction.West), 3);
			GameManager.Instance.EnqueueResolver(new UnloadDeckResolver(DeckType.EastDeck, Direction.West), 3);
			GameManager.Instance.EnqueueResolver(new DecideActionResolver(30), 3);
			GameManager.Instance.EnqueueResolver(new WaitActionResolver(), 3);
			GameManager.Instance.EnqueueResolver(new RevealActionResolver(Direction.West), 3);
			GameManager.Instance.EnqueueResolver(new PerformActionResolver(Direction.West), 3);
		}
		GameManager.Instance.EnqueueResolver(new UnloadHandResolver(DeckType.Discard, Direction.West), 3);

		yield return null;
	}

}
