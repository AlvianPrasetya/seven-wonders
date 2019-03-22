using System;
using System.Collections;
using UnityEngine;

public abstract class Slot<T> : MonoBehaviour, IPushable<T>, IPoppable<T> where T : MonoBehaviour, IMoveable {

	public Vector3 dropSpacing = new Vector3(0, 1, 0);
	public Facing facing = Facing.Up;
	public float pushDuration = 1;
	public T Element { get; private set; }

	public IEnumerator Push(T element) {
		Vector3 dropPosition = transform.position + dropSpacing;
		Vector3 dropEulerAngles = transform.rotation.eulerAngles;
		if (facing == Facing.Up) {
			dropEulerAngles.z = 0.0f;
		} else {
			dropEulerAngles.z = 180.0f;
		}
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);

		yield return element.MoveTowards(dropPosition, dropRotation, pushDuration);

		Element = element;
		element.transform.parent = transform;
	}

	public IEnumerator PushMany(T[] elements) {
		throw new NotImplementedException("PushMany is not supported for Slot");
	}

	public T Pop() {
		T poppedElement = Element;
		Element = null;
		return poppedElement;
	}

	public T[] PopMany(int count) {
		throw new NotImplementedException("PopMany is not supported for Slot");
	}

	public IEnumerator Flip() {
		facing = (facing == Facing.Up) ? Facing.Down : Facing.Up;
		if (Element != null) {
			yield return Push(Pop());
		}
	}

}
