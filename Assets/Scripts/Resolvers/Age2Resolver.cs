using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age2Resolver : IResolvable {

	private const int TurnCount = 6;

	public IEnumerator Resolve() {
		Queue<Coroutine> unloadHands = new Queue<Coroutine>();
		Queue<Coroutine> unloadDecks = new Queue<Coroutine>();
		
		foreach (Player player in GameManager.Instance.players) {
			unloadDecks.Enqueue(GameManager.Instance.StartCoroutine(
				player.Decks[DeckType.Age2].Unload(player.hand, Direction.East)
			));
		}
		while (unloadDecks.Count != 0) {
			yield return unloadDecks.Dequeue();
		}
		
		for (int i = 0; i < TurnCount - 1; i++) {
			foreach (Player player in GameManager.Instance.players) {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					player.hand.Unload(player.eastNeighbour.Decks[DeckType.WestDeck], Direction.East)
				));
			}
			while (unloadHands.Count != 0) {
				yield return unloadHands.Dequeue();
			}

			foreach (Player player in GameManager.Instance.players) {
				unloadDecks.Enqueue(GameManager.Instance.StartCoroutine(
					player.Decks[DeckType.WestDeck].Unload(player.hand, Direction.East)
				));
			}
			while (unloadDecks.Count != 0) {
				yield return unloadDecks.Dequeue();
			}
		}
		
		foreach (Player player in GameManager.Instance.players) {
			unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
				player.hand.Unload(GameManager.Instance.discardPile, Direction.East)
			));
		}
		while (unloadHands.Count != 0) {
			yield return unloadHands.Dequeue();
		}
	}

}
