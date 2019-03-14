using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of cards with stack-like behaviours.
public class Stock : CardPile {

	public Stack<Card> Cards { get; private set; }

	void Awake() {
		Cards = new Stack<Card>();
	}

	void Start() {
		StartCoroutine(LoadCards());
	}

	public override void Push(Card card) {
		Vector3 dropPosition = transform.position + transform.up * 0.5f * (Cards.Count + 1);
		StartCoroutine(card.MoveTowards(dropPosition, card.transform.rotation));

		Cards.Push(card);
	}

	public override Card Pop() {
		return Cards.Pop();
	}

	protected override IEnumerator LoadCards() {
		foreach (Card cardPrefab in cardPrefabs) {
			Card card = Instantiate(
				cardPrefab,
				transform.position + transform.up * 0.5f * (Cards.Count + 1),
				transform.rotation
			);
			Push(card);

			yield return new WaitForSeconds(0.05f);
		}
	}

}
