using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadHandResolver : IResolvable {

	private DeckType deckType;
	private Direction unloadDirection;
	private bool toSelf;
	private Player targetPlayer;

	public UnloadHandResolver(
		DeckType deckType, Direction unloadDirection,
		bool toSelf = false, Player targetPlayer = null
	) {
		this.deckType = deckType;
		this.unloadDirection = unloadDirection;
		this.toSelf = toSelf;
		this.targetPlayer = targetPlayer;
	}

	public IEnumerator Resolve() {
		Queue<Player> targetPlayers = new Queue<Player>();
		if (targetPlayer != null) {
			// Resolve for the target player only
			targetPlayers.Enqueue(targetPlayer);
		} else {
			// Resolve for all players
			foreach (Player player in GameManager.Instance.Players) {
				targetPlayers.Enqueue(player);
			}
		}

		Queue<Coroutine> unloadHands = new Queue<Coroutine>();
		while (targetPlayers.Count != 0) {
			Player targetPlayer = targetPlayers.Dequeue();
			if (toSelf) {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					targetPlayer.hand.Unload(
						targetPlayer.Decks[deckType],
						unloadDirection
					)
				));
			} else {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					targetPlayer.hand.Unload(
						targetPlayer.Neighbours[unloadDirection].Decks[deckType],
						unloadDirection
					)
				));
			}
		}
		while (unloadHands.Count != 0) {
			yield return unloadHands.Dequeue();
		}
	}
	
}
