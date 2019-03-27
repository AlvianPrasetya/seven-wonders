using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pile represents a collection of moveable elements arranged in a stack-like manner.
public abstract class Pile<T> : MonoBehaviour, IPushable<T>, IPoppable<T>, IPeekable<T>, IDumpable<T> where T : MonoBehaviour, IMoveable {

	public Vector3 initialSpacing = new Vector3(0, 0.025f, 0);
	public Vector3 spacing = new Vector3(0, 0.05f, 0);
	public Facing facing = Facing.Up;
	public float pushDuration = 0.5f;
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
		Vector3 dropPosition = transform.position + 
			transform.right * (initialSpacing.x + spacing.x * (Elements.Count)) +
			transform.up * (initialSpacing.y + spacing.y * (Elements.Count)) + 
			transform.forward * (initialSpacing.z + spacing.z * (Elements.Count));
		Vector3 dropEulerAngles = transform.rotation.eulerAngles;
		if (facing == Facing.Up) {
			dropEulerAngles.z = 0.0f;
		} else {
			dropEulerAngles.z = 180.0f;
		}
		Quaternion dropRotation = Quaternion.Euler(dropEulerAngles);

		Elements.Push(element);
		element.transform.parent = transform;
		yield return element.MoveTowards(dropPosition, dropRotation, pushDuration);
	}

	public IEnumerator PushMany(T[] elements) {
		foreach (T element in elements) {
			yield return Push(element);
		}
	}

	public T Pop() {
		if (Elements.Count == 0) {
			return null;
		}

		T poppedElement = Elements.Pop();
		poppedElement.transform.parent = null;
		return poppedElement;
	}

	public T[] PopMany(int count) {
		List<T> poppedElements = new List<T>();
		while (poppedElements.Count != count && Count != 0) {
			poppedElements.Add(Pop());
		}

		return poppedElements.ToArray();
	}

	public T Peek() {
		if (Elements.Count == 0) {
			return null;
		}

		return Elements.Peek();
	}

	public IEnumerator Dump() {
		while (Elements.Count != 0) {
			T element = Elements.Pop();
			Destroy(element.gameObject);
			yield return new WaitForSeconds(pushDuration);
		}
	}

}
