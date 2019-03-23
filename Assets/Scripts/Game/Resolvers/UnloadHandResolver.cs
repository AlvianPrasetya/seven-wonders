using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadHandResolver : IResolvable {

	private DeckType deckType;
	private Direction unloadDirection;

	public UnloadHandResolver(DeckType deckType, Direction unloadDirection) {
		this.deckType = deckType;
		this.unloadDirection = unloadDirection;
	}

	public IEnumerator Resolve() {
		// Focus on local player when unloading hand
		yield return GameManager.Instance.gameCamera.Focus(GameManager.Instance.Player);

		Queue<Coroutine> unloadHands = new Queue<Coroutine>();
		foreach (Player player in GameManager.Instance.Players) {
			if (deckType == DeckType.Discard) {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					player.hand.Unload(GameManager.Instance.discardPile, unloadDirection)
				));
			} else {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					player.hand.Unload(player.Neighbours[unloadDirection].Decks[deckType], unloadDirection)
				));
			}
		}
		while (unloadHands.Count != 0) {
			yield return unloadHands.Dequeue();
		}
	}
	
}
