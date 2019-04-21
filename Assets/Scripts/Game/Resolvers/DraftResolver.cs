using Photon.Pun;
using System.Collections;

public class DraftResolver : IResolvable {

	private DeckType? sourceDeck;
	private DeckType? targetDeck;
	private Direction direction;

	public DraftResolver(DeckType? sourceDeck, DeckType? targetDeck, Direction direction) {
		this.sourceDeck = sourceDeck;
		this.targetDeck = targetDeck;
		this.direction = direction;
	}

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		if (sourceDeck != null) {
			GameManager.Instance.EnqueueResolver(
				new UnloadDeckResolver(sourceDeck ?? default(DeckType), direction),
				Priority.PlayHand
			);
		}

		if (PhotonNetwork.IsMasterClient) {
			foreach (Bot bot in GameManager.Instance.Bots) {
				GameManager.Instance.EnqueueResolver(
					new DecideBotDraftResolver(bot),
					Priority.PlayHand
				);
			}
		}
		GameManager.Instance.EnqueueResolver(
			new DecideDraftResolver(GameOptions.DecideTime),
			Priority.PlayHand
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		GameManager.Instance.EnqueueResolver(new PerformActionResolver(), Priority.PlayHand);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(), Priority.PlayHand);
		
		if (targetDeck != null) {
			GameManager.Instance.EnqueueResolver(
				new UnloadHandResolver(targetDeck ?? default(DeckType), direction),
				Priority.PlayHand
			);
		}
		
		yield return null;
	}

}
