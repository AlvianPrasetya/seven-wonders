using Photon.Pun;
using System.Collections;

public class TurnResolver : IResolvable {

	public static Player DoubleTurnPlayer { private get; set; }

	private DeckType? sourceDeck;
	private DeckType? targetDeck;
	private Direction direction;
	private bool targetSelf;

	public TurnResolver(DeckType? sourceDeck, DeckType? targetDeck, Direction direction, bool targetSelf = false) {
		this.sourceDeck = sourceDeck;
		this.targetDeck = targetDeck;
		this.direction = direction;
		this.targetSelf = targetSelf;
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
					new DecideBotActionResolver(bot),
					Priority.PlayHand
				);
			}
		}
		GameManager.Instance.EnqueueResolver(
			new DecideActionResolver(GameOptions.DecideTime),
			Priority.PlayHand
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		GameManager.Instance.EnqueueResolver(new PerformActionResolver(), Priority.PlayHand);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(), Priority.PlayHand);
		
		if (targetDeck != null) {
			GameManager.Instance.EnqueueResolver(
				new UnloadHandResolver(targetDeck ?? default(DeckType), direction, targetSelf), Priority.PlayHand
			);
		}
		
		yield return null;
	}

}
