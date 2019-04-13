using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeStock : Stock<Card>, IDealable {

	public Age age;
	public DeckType dealDeckType;
	public CardPile[] cardRifflePiles;

	protected override void Awake() {
		base.Awake();
		rifflePiles = cardRifflePiles;
	}

	public override IEnumerator Load() {
		CardStock rawMaterialStock = GameManager.Instance.CardStocks[StockType.RawMaterial];
		CardStock manufacturedGoodsStock = GameManager.Instance.CardStocks[StockType.ManufacturedGoods];
		CardStock civilianStock = GameManager.Instance.CardStocks[StockType.Civilian];
		CardStock scientificStock = GameManager.Instance.CardStocks[StockType.Scientific];
		CardStock commercialStock = GameManager.Instance.CardStocks[StockType.Commercial];
		CardStock militaryStock = GameManager.Instance.CardStocks[StockType.Military];
		CardStock guildStock = GameManager.Instance.CardStocks[StockType.Guild];

		while (rawMaterialStock.Count != 0) {
			Card card = rawMaterialStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return rawMaterialStock.Push(card);
				break;
			}
		}

		while (manufacturedGoodsStock.Count != 0) {
			Card card = manufacturedGoodsStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return manufacturedGoodsStock.Push(card);
				break;
			}
		}

		while (civilianStock.Count != 0) {
			Card card = civilianStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return civilianStock.Push(card);
				break;
			}
		}

		while (scientificStock.Count != 0) {
			Card card = scientificStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return scientificStock.Push(card);
				break;
			}
		}

		while (commercialStock.Count != 0) {
			Card card = commercialStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return commercialStock.Push(card);
				break;
			}
		}

		while (militaryStock.Count != 0) {
			Card card = militaryStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return militaryStock.Push(card);
				break;
			}
		}

		// N + 2 guild cards
		int guildsCount = GameManager.Instance.Players.Count + 2;
		for (int i = 0; i < guildsCount; i++) {
			Card card = guildStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return guildStock.Push(card);
				break;
			}
		}
	}

	public IEnumerator Deal() {
		int playerIndex = 0;
		Queue<Coroutine> dealCards = new Queue<Coroutine>();
		while (Elements.Count != 0) {
			dealCards.Enqueue(StartCoroutine(
				GameManager.Instance.Players[playerIndex].Decks[dealDeckType].Push(Pop())
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
