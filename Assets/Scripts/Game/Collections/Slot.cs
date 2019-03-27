using System;
using System.Collections;
using UnityEngine;

public abstract class Slot<T> : MonoBehaviour, IPushable<T>, IPoppable<T> where T : MonoBehaviour, IMoveable {

	public Vector3 spacing = new Vector3(0, 0.025f, 0);
	public Facing facing = Facing.Up;
	public float pushDuration = 1;
	public T Element { get; private set; }

	public IEnumerator Push(T element) {
		Vector3 dropPosition = transform.position + 
			transform.right * spacing.x +
			transform.up * spacing.y + 
			transform.forward * spacing.z;
		Vector3 dropEulerAngles = transform.rotation.eulerAngles;
		if (facing == Facing.Up) {
			dropEulerAngles.z = 0.0f;
		} else {
			dropEulerAngles.z = 180.0f;
		}
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);

		Element = element;
		element.transform.parent = transform;
		yield return element.MoveTowards(dropPosition, dropRotation, pushDuration);
	}

	public IEnumerator PushMany(T[] elements) {
		throw new NotImplementedException("PushMany is not supported for Slot");
	}

	public T Pop() {
		T poppedElement = Element;
		Element = null;
		poppedElement.transform.parent = null;
		return poppedElement;
	}

	public T[] PopMany(int count) {
		throw new NotImplementedException("PopMany is not supported for Slot");
	}

}
