using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Age1Resolver : IResolvable {

	public IEnumerator Resolve() {
		List<Coroutine> unloadAge1Decks = new List<Coroutine>();
		foreach (Player player in GameManager.Instance.players) {
			unloadAge1Decks.Add(GameManager.Instance.StartCoroutine(
				player.Decks[DeckType.Age1].Unload(player.hand)
			));
		}

		foreach (Coroutine unloadAge1Deck in unloadAge1Decks) {
			yield return unloadAge1Deck;
		}
	}

}
