using System.Collections;

public class DraftingPhaseResolver : IResolvable {

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(
			new DraftResolver(DeckType.Leader, DeckType.West, Direction.East),
			Priority.ResolveTurn
		);
		for (int i = 0; i < GameOptions.DraftCount - 2; i++) {
			GameManager.Instance.EnqueueResolver(
				new DraftResolver(DeckType.West, DeckType.West, Direction.East),
				Priority.ResolveTurn
			);
		}
		GameManager.Instance.EnqueueResolver(
			new DraftResolver(DeckType.West, DeckType.Discard, Direction.East),
			Priority.ResolveTurn
		);

		yield return null;
	}

}
