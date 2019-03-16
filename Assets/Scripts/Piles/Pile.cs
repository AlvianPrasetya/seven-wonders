using System.Collections;
using UnityEngine;

// Pile represents a collection of elements arranged in a stack-like manner.
public abstract class Pile<T> : MonoBehaviour {

	public abstract int Count { get; }

	public abstract IEnumerator Push(T element);
	public abstract T Pop();

}
