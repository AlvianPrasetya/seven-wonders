using UnityEngine;
using UnityEngine.Events;

public abstract class PlayArea : MonoBehaviour {

	private new Renderer renderer;
	private new Collider collider;
	
	public bool IsPlayable {
		set {
			renderer.enabled = value;
			collider.enabled = value;
		}
	}

	void Awake() {
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public abstract void Play(Card card);

}
