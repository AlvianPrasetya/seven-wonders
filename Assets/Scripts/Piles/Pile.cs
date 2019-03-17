using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pile represents a collection of elements arranged in a stack-like manner.
public abstract class Pile<T> : MonoBehaviour {

	public abstract int Count { get; }

	public abstract IEnumerator Push(T element);
	public abstract T Pop();
	public IEnumerator PushMany(T[] elements) {
		foreach (T element in elements) {
			yield return Push(element);
		}
	}
	public T[] PopMany(int count) {
		List<T> poppedElements = new List<T>();
		while (poppedElements.Count != count && Count != 0) {
			poppedElements.Add(Pop());
		}

		return poppedElements.ToArray();
	}

}
