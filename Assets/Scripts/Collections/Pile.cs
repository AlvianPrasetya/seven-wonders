using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pile represents a collection of moveable elements arranged in a stack-like manner.
public abstract class Pile<T> : MonoBehaviour where T : MonoBehaviour, IMoveable {

	public Vector3 dropSpacing = new Vector3(0, 0.2f, 0);
	public Facing facing = Facing.Up;
	public Stack<T> Elements { get; protected set; }
	public int Count {
		get {
			return Elements.Count;
		}
	}

	void Awake() {
		Elements = new Stack<T>();
	}

	public virtual IEnumerator Push(T element) {
		Vector3 dropPosition = transform.position + dropSpacing * (Elements.Count + 1);
		Vector3 dropEulerAngles = transform.rotation.eulerAngles;
		if (facing == Facing.Up) {
			dropEulerAngles.z = 0.0f;
		} else {
			dropEulerAngles.z = 180.0f;
		}
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);

		yield return element.MoveTowards(dropPosition, dropRotation);

		Elements.Push(element);
		element.transform.parent = transform;
	}

	public virtual T Pop() {
		if (Elements.Count == 0) {
			return null;
		}

		return Elements.Pop();
	}

	public IEnumerator PushMany(IMoveable[] elements) {
		foreach (T element in elements) {
			yield return Push(element);
		}
	}

	public IMoveable[] PopMany(int count) {
		List<IMoveable> poppedElements = new List<IMoveable>();
		while (poppedElements.Count != count && Count != 0) {
			poppedElements.Add(Pop());
		}

		return poppedElements.ToArray();
	}

}
