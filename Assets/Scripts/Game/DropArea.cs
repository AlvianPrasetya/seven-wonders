using UnityEngine;
using UnityEngine.Events;

public abstract class DropArea<T> : MonoBehaviour {

	private new Renderer renderer;
	private new Collider collider;
	
	public bool IsActive {
		set {
			renderer.enabled = value;
			collider.enabled = value;
		}
	}

	void Awake() {
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public abstract void Drop(T droppedItem);

}
