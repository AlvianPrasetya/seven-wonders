using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadHandResolver : IResolvable {

	private DeckType deckType;
	private Direction unloadDirection;
	private Player targetPlayer;

	public UnloadHandResolver(DeckType deckType, Direction unloadDirection, Player targetPlayer = null) {
		this.deckType = deckType;
		this.unloadDirection = unloadDirection;
		this.targetPlayer = targetPlayer;
	}

	public IEnumerator Resolve() {
		if (targetPlayer != null) {
			// Focus on the only player unloading hand
			yield return GameManager.Instance.gameCamera.Focus(targetPlayer);
			// Unload hand to the player's deck
			yield return targetPlayer.hand.Unload(targetPlayer.Decks[deckType], unloadDirection);
		} else {
			// Focus on local player when unloading hand
			yield return GameManager.Instance.gameCamera.Focus(GameManager.Instance.Player);
			// Unload each player's hand to the neighbour's deck
			Queue<Coroutine> unloadHands = new Queue<Coroutine>();
			foreach (Player player in GameManager.Instance.Players) {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					player.hand.Unload(
						player.Neighbours[unloadDirection].Decks[deckType], unloadDirection
					)
				));
			}
			while (unloadHands.Count != 0) {
				yield return unloadHands.Dequeue();
			}
		}
	}
	
}
