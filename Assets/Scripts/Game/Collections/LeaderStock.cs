using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderStock : CardStock {

	public override IEnumerator Deal() {
		Queue<Coroutine> dealCards = new Queue<Coroutine>();
		for (int i = 0; i < GameOptions.DraftCount + 1; i++) {
			foreach (Player player in GameManager.Instance.Players) {
				dealCards.Enqueue(StartCoroutine(
					player.Decks[DeckType.Leader].Push(Pop())
				));
			}

			while (dealCards.Count != 0) {
				yield return dealCards.Dequeue();
			}
		}

		yield return Dump();
	}

}
