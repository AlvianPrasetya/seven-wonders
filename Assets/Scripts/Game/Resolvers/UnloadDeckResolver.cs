using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadDeckResolver : IResolvable {
	
	private DeckType deckType;
	private Direction pushDirection;

	public UnloadDeckResolver(DeckType deckType, Direction pushDirection) {
		this.deckType = deckType;
		this.pushDirection = pushDirection;
	}

	public IEnumerator Resolve() {
		// Focus on local player when unloading deck
		yield return GameManager.Instance.gameCamera.Focus(GameManager.Instance.Player);

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
