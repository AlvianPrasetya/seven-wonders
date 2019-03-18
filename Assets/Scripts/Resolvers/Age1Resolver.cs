using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age1Resolver : IResolvable {

	private const int TurnCount = 6;

	public IEnumerator Resolve() {
		Queue<Coroutine> unloadHands = new Queue<Coroutine>();
		Queue<Coroutine> unloadDecks = new Queue<Coroutine>();
		
		foreach (Player player in GameManager.Instance.players) {
			unloadDecks.Enqueue(GameManager.Instance.StartCoroutine(
				player.Decks[DeckType.Age1].Unload(player.hand, Direction.West)
			));
		}
		while (unloadDecks.Count != 0) {
			yield return unloadDecks.Dequeue();
		}
		
		for (int i = 0; i < TurnCount - 1; i++) {
			foreach (Player player in GameManager.Instance.players) {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					player.hand.Unload(player.westNeighbour.Decks[DeckType.EastDeck], Direction.West)
				));
			}
			while (unloadHands.Count != 0) {
				yield return unloadHands.Dequeue();
			}

			foreach (Player player in GameManager.Instance.players) {
				unloadDecks.Enqueue(GameManager.Instance.StartCoroutine(
					player.Decks[DeckType.EastDeck].Unload(player.hand, Direction.West)
				));
			}
			while (unloadDecks.Count != 0) {
				yield return unloadDecks.Dequeue();
			}
		}
		
		foreach (Player player in GameManager.Instance.players) {
			yield return player.hand.Unload(GameManager.Instance.discardPile, Direction.West);
		}
	}

}
