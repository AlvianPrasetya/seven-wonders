using UnityEngine;
using UnityEngine.Events;

public abstract class DropArea<T> : MonoBehaviour {

	public Material normalMaterial;
	public Material highlightedMaterial;

	private new Renderer renderer;
	private new Collider collider;
	
	public bool IsPlayable {
		set {
			renderer.enabled = value;
			collider.enabled = value;
		}
	}

	public bool IsHighlighted {
		set {
			renderer.material = (value) ? highlightedMaterial : normalMaterial;
		}
	}

	void Awake() {
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public abstract void Drop(T droppedItem);

}
