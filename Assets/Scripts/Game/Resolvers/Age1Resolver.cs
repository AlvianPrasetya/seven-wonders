using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age1Resolver : IResolvable {

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(
			TurnResolverFactory.Instance.Create(DeckType.Age1, DeckType.East, Direction.West),
			Priority.ResolveTurn
		);
		for (int i = 0; i < GameOptions.TurnsPerAge - 2; i++) {
			GameManager.Instance.EnqueueResolver(
				TurnResolverFactory.Instance.Create(DeckType.East, DeckType.East, Direction.West),
				Priority.ResolveTurn
			);
		}
		GameManager.Instance.EnqueueResolver(
			TurnResolverFactory.Instance.Create(DeckType.East, DeckType.Discard, Direction.West),
			Priority.ResolveTurn
		);

		yield return null;
	}

}
