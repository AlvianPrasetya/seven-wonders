using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// JunkPile represents a pile of junk/unused cards.
public class JunkPile : Pile<Card> {

	private const float DropSpacing = 0.2f;
	
	public Facing facing;

	void Awake() {
		Elements = new LinkedList<Card>();
	}

	public override IEnumerator Push(Card card) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Elements.Count + 1);
		Vector3 dropEulerAngles = transform.rotation.eulerAngles;
		if (facing == Facing.Down) {
			dropEulerAngles.z = 180.0f;
		}
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);
		yield return card.MoveTowards(dropPosition, dropRotation, 100, 360);

		Elements.AddLast(card);
		card.transform.parent = transform;
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
