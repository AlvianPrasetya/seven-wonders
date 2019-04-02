using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadDeckResolver : IResolvable {
	
	private DeckType deckType;
	private Direction pushDirection;
	private Player targetPlayer;

	public UnloadDeckResolver(DeckType deckType, Direction pushDirection, Player targetPlayer = null) {
		this.deckType = deckType;
		this.pushDirection = pushDirection;
		this.targetPlayer = targetPlayer;
	}

	public IEnumerator Resolve() {
		if (targetPlayer != null) {
			// Focus on the only player unloading deck
			yield return GameManager.Instance.gameCamera.Focus(targetPlayer);
			// Unload deck to the player's hand
			yield return targetPlayer.Decks[deckType].Unload(targetPlayer.hand, pushDirection);
		} else {
			// Focus on local player when unloading deck
			yield return GameManager.Instance.gameCamera.Focus(GameManager.Instance.Player);
			// Unload each player's deck to the player's hand
			Queue<Coroutine> unloadDecks = new Queue<Coroutine>();
			foreach (Player player in GameManager.Instance.Players) {
				unloadDecks.Enqueue(GameManager.Instance.StartCoroutine(
					player.Decks[deckType].Unload(player.hand, pushDirection)
				));
			}
			while (unloadDecks.Count != 0) {
				yield return unloadDecks.Dequeue();
			}
		}
	}

}
