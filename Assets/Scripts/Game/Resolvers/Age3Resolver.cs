using System.Collections;

public class Age3Resolver : IResolvable {

	public IEnumerator Resolve() {
		GameManager.Instance.CurrentAge = Age.Age3;
		
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.Leader, DeckType.Leader, Direction.West, true),
			Priority.ResolveTurn
		);
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.Age3, DeckType.East, Direction.West),
			Priority.ResolveTurn
		);
		for (int i = 0; i < GameOptions.TurnsPerAge - 2; i++) {
			GameManager.Instance.EnqueueResolver(
				new TurnResolver(DeckType.East, DeckType.East, Direction.West),
				Priority.ResolveTurn
			);
		}
		GameManager.Instance.EnqueueResolver(
			new TurnResolver(DeckType.East, null, Direction.West),
			Priority.ResolveTurn
		);

		GameManager.Instance.EnqueueResolver(
			new ExtraTurnResolver(null, DeckType.Discard, Direction.West),
			Priority.ResolveTurn
		);

		GameManager.Instance.EnqueueResolver(
			new MilitaryConflictResolver(
				GameManager.Instance.victoryTokenAge3Prefab,
				GameManager.Instance.drawTokenPrefab,
				GameManager.Instance.defeatTokenPrefab
			),
			Priority.ResolveTurn
		);

		yield return null;
	}

}
