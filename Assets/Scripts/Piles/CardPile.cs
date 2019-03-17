using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CardPile represents a pile cards.
public class CardPile : Pile<Card> {

	private const float DropSpacing = 0.2f;
	
	public Facing facing;
	public Stack<Card> Cards { get; protected set; }
	public override int Count {
		get {
			return Cards.Count;
		}
	}

	void Awake() {
		Cards = new Stack<Card>();
	}

	public override IEnumerator Push(Card card) {
		Vector3 dropPosition = transform.position + transform.up * DropSpacing * (Cards.Count + 1);
		Vector3 dropEulerAngles = transform.rotation.eulerAngles;
		if (facing == Facing.Up) {
			dropEulerAngles.z = 0.0f;
		} else {
			dropEulerAngles.z = 180.0f;
		}
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);
		yield return card.MoveTowards(dropPosition, dropRotation, 100, 360);

		Cards.Push(card);
		card.transform.parent = transform;
	}

	public override Card Pop() {
		if (Cards.Count == 0) {
			return null;
		}

		return Cards.Pop();
	}

}
