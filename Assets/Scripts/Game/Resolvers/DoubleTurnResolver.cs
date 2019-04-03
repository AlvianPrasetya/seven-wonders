using Photon.Pun;
using System.Collections;

public class DoubleTurnResolver : IResolvable {

	private DeckType sourceDeck;
	private DeckType targetDeck;
	private Direction direction;
	private Player doubleTurnPlayer;

	public DoubleTurnResolver(
		DeckType sourceDeck, DeckType targetDeck, Direction direction, Player doubleTurnPlayer
	) {
		this.sourceDeck = sourceDeck;
		this.targetDeck = targetDeck;
		this.direction = direction;
		this.doubleTurnPlayer = doubleTurnPlayer;
	}

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(
			new UnloadDeckResolver(sourceDeck, direction),
			Priority.PlayHand
		);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

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
		
		if (doubleTurnPlayer == GameManager.Instance.Player) {
			// Is the player with double turn, enqueue decide action resolver
			GameManager.Instance.EnqueueResolver(
				new DecideActionResolver(GameOptions.DecideTime),
				Priority.PlayHand
			);
		}
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority.PlayHand);

		GameManager.Instance.EnqueueResolver(new PerformActionResolver(doubleTurnPlayer), Priority.PlayHand);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(doubleTurnPlayer), Priority.PlayHand);
		
		GameManager.Instance.EnqueueResolver(
			new UnloadHandResolver(targetDeck, direction),
			(targetDeck == DeckType.Discard) ? Priority.DiscardLastHand : Priority.PlayHand
		);
		
		yield return null;
	}

}
