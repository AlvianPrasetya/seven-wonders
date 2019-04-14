using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderStock : Stock<Card>, IDealable {

	public CardPile[] cardRifflePiles;

	protected override void Awake() {
		base.Awake();
		rifflePiles = cardRifflePiles;
	}

	public override IEnumerator Load() {
		CardStock leaderStock = GameManager.Instance.CardStocks[StockType.Leader];

		// N * 4 leader cards
		int leadersCount = GameManager.Instance.Players.Count * 4;
		for (int i = 0; i < leadersCount; i++) {
			Card card = leaderStock.Pop();
			yield return Push(card);
		}
	}

	public IEnumerator Deal() {
		int playerIndex = 0;
		Queue<Coroutine> dealCards = new Queue<Coroutine>();
		while (Elements.Count != 0) {
			dealCards.Enqueue(StartCoroutine(
				GameManager.Instance.Players[playerIndex].Decks[DeckType.Leader].Push(Pop())
			));
			playerIndex = (playerIndex + 1) % GameManager.Instance.Players.Count;

			if (dealCards.Count == GameManager.Instance.Players.Count) {
				// Deal to all players at a time
				while (dealCards.Count != 0) {
					yield return dealCards.Dequeue();
				}
			}
		}
		while (dealCards.Count != 0) {
			yield return dealCards.Dequeue();
		}
	}

}
