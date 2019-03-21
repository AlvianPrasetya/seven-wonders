using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age2Resolver : IResolvable {

	private const int TurnCount = 6;

	public IEnumerator Resolve() {
		Queue<Coroutine> unloadHands = new Queue<Coroutine>();
		Queue<Coroutine> unloadDecks = new Queue<Coroutine>();
		
		foreach (Player player in GameManager.Instance.Players) {
			unloadDecks.Enqueue(GameManager.Instance.StartCoroutine(
				player.Decks[DeckType.Age2].Unload(player.hand, Direction.East)
			));
		}
		while (unloadDecks.Count != 0) {
			yield return unloadDecks.Dequeue();
		}

		// Simulate all players discarding easternmost card
		for (int i = 0; i < GameManager.Instance.Players.Count; i++) {
			yield return GameManager.Instance.discardPile.Push(
				GameManager.Instance.Players[i].hand.PopAt(
					GameManager.Instance.Players[i].hand.displayPiles.Length - 1
				)
			);
		}
		
		for (int i = 0; i < TurnCount - 1; i++) {
			foreach (Player player in GameManager.Instance.Players) {
				unloadHands.Enqueue(GameManager.Instance.StartCoroutine(
					player.hand.Unload(player.eastNeighbour.Decks[DeckType.WestDeck], Direction.East)
				));
			}
			while (unloadHands.Count != 0) {
				yield return unloadHands.Dequeue();
			}

			foreach (Player player in GameManager.Instance.Players) {
				unloadDecks.Enqueue(GameManager.Instance.StartCoroutine(
					player.Decks[DeckType.WestDeck].Unload(player.hand, Direction.East)
				));
			}
			while (unloadDecks.Count != 0) {
				yield return unloadDecks.Dequeue();
			}

			// Simulate all players discarding easternmost card
			for (int j = 0; j < GameManager.Instance.Players.Count; j++) {
				yield return GameManager.Instance.discardPile.Push(
					GameManager.Instance.Players[j].hand.PopAt(
						GameManager.Instance.Players[j].hand.displayPiles.Length - 1
					)
				);
			}
		}
		
		foreach (Player player in GameManager.Instance.Players) {
			yield return player.hand.Unload(GameManager.Instance.discardPile, Direction.East);
		}
	}

}
