using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : Pile<Card> {

	private const float DropSpacing = 0.25f;
	
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
		// Cards in a deck should always be facing down
		dropEulerAngles.z = 180.0f;
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);
		yield return card.MoveTowards(dropPosition, dropRotation, 200, 1080);

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
