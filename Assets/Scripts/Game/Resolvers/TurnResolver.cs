using Photon.Pun;
using System.Collections;

public class TurnResolver : IResolvable {

	private const int Priority = 5;

	private DeckType sourceDeck;
	private DeckType targetDeck;
	private Direction direction;

	public TurnResolver(DeckType sourceDeck, DeckType targetDeck, Direction direction) {
		this.sourceDeck = sourceDeck;
		this.targetDeck = targetDeck;
		this.direction = direction;
	}

	public IEnumerator Resolve() {
		GameManager.Instance.EnqueueResolver(new UnloadDeckResolver(sourceDeck, direction), Priority);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority);

		if (PhotonNetwork.IsMasterClient) {
			foreach (Bot bot in GameManager.Instance.Bots) {
				GameManager.Instance.EnqueueResolver(new DecideBotActionResolver(bot), Priority);
			}
		}
		GameManager.Instance.EnqueueResolver(new DecideActionResolver(GameOptions.DecideTime), Priority);
		GameManager.Instance.EnqueueResolver(new SyncResolver(), Priority);

		GameManager.Instance.EnqueueResolver(new PerformActionResolver(), Priority);
		GameManager.Instance.EnqueueResolver(new EffectActionResolver(), Priority);
		GameManager.Instance.EnqueueResolver(new UnloadHandResolver(targetDeck, direction), Priority);
		
		yield return null;
	}

}
