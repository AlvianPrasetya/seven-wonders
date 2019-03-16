using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// JunkPile represents a pile of junk/unused cards.
public class JunkPile : Pile<Card> {

	private const float DropSpacing = 0.2f;

	void Awake() {
		Elements = new LinkedList<Card>();
	}

	public override IEnumerator Push(Card card) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Elements.Count + 1);
		yield return card.MoveTowards(dropPosition, transform.rotation, 100, 360);

		Elements.AddLast(card);
	}

	public override Card Pop() {
		if (Elements.Count == 0) {
			return null;
		}

		Card topCard = Elements.Last.Value;
		Elements.RemoveLast();
		return topCard;
	}

}
