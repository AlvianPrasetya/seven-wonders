using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPile : CardPile {

	public override IEnumerator Push(Card newCard) {
		bool pushedNewCard = false;
		Stack<Card> tempStack = new Stack<Card>();
		while (Elements.Count != 0) {
			Card card = Pop();
			if (!pushedNewCard && newCard.displayPriority >= card.displayPriority) {
				tempStack.Push(newCard);
				pushedNewCard = true;
			}
			tempStack.Push(card);
		}
		if (!pushedNewCard) {
			tempStack.Push(newCard);
			pushedNewCard = true;
		}

		Queue<Coroutine> dropCards = new Queue<Coroutine>();
		while (tempStack.Count != 0) {
			Card card = tempStack.Pop();
			Vector3 dropPosition = transform.position + 
				transform.right * (initialSpacing.x + spacing.x * (Elements.Count)) +
				transform.up * (initialSpacing.y + spacing.y * (tempStack.Count)) + 
				transform.forward * (initialSpacing.z + spacing.z * (Elements.Count));
			Vector3 dropEulerAngles = transform.rotation.eulerAngles;
			if (facing == Facing.Down) {
				dropEulerAngles.z += 180;
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
