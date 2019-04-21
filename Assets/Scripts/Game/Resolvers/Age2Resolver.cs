using System.Collections;

public class Age2Resolver : IResolvable {

	public IEnumerator Resolve() {
		GameManager.Instance.CurrentAge = Age.Age2;
		
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.Leader, DeckType.Leader, Direction.East, true),
			Priority.ResolveTurn
		);
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.Age2, DeckType.West, Direction.East),
			Priority.ResolveTurn
		);
		for (int i = 0; i < GameOptions.TurnsPerAge - 2; i++) {
			GameManager.Instance.EnqueueResolver(
				new TurnResolver(DeckType.West, DeckType.West, Direction.East),
				Priority.ResolveTurn
			);
		}
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.West, null, Direction.East),
			Priority.ResolveTurn
		);

		GameManager.Instance.EnqueueResolver(
			new ExtraTurnResolver(null, DeckType.Discard, Direction.East),
			Priority.ResolveTurn
		);

		GameManager.Instance.EnqueueResolver(
			new MilitaryConflictResolver(
				GameManager.Instance.victoryTokenAge2Prefab,
				GameManager.Instance.drawTokenPrefab,
				GameManager.Instance.defeatTokenPrefab
			),
			Priority.ResolveTurn
		);

		yield return null;
	}

}
