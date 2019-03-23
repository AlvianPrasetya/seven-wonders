using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPile : CardPile {

	public override IEnumerator Push(Card newCard) {
		Stack<Card> tempStack = new Stack<Card>();
		tempStack.Push(newCard);
		while (Elements.Count != 0) {
			tempStack.Push(Pop());
		}

		Queue<Coroutine> dropCards = new Queue<Coroutine>();
		while (tempStack.Count != 0) {
			Card card = tempStack.Pop();
			Vector3 dropPosition = transform.position + 
				transform.right * dropSpacing.x * (Elements.Count + 1) +
				transform.up * dropSpacing.y * (tempStack.Count + 1) + 
				transform.forward * dropSpacing.z * (Elements.Count + 1);
			Vector3 dropEulerAngles = transform.rotation.eulerAngles;
			if (facing == Facing.Up) {
				dropEulerAngles.z = 0.0f;
			} else {
				dropEulerAngles.z = 180.0f;
			}
			Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);

			dropCards.Enqueue(StartCoroutine(
				card.MoveTowards(dropPosition, dropRotation, pushDuration)
			));

			Elements.Push(card);
			card.transform.parent = transform;
		}
		while (dropCards.Count != 0) {
			yield return dropCards.Dequeue();
		}
	}

}
