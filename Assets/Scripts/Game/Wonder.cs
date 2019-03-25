using System.Collections;
using UnityEngine;

public class Wonder : MonoBehaviour, IMoveable {
	
	public WonderStage[] wonderStages;

	protected new Collider collider;
	protected new Rigidbody rigidbody;

	void Awake() {
		collider = GetComponent<Collider>();
	}

	public IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation, float duration) {
		collider.enabled = false;

		Vector3 initialPosition = transform.position;
		Quaternion initialRotation = transform.rotation;

		float progress = 0;
		while (progress < 1) {
			progress = Mathf.Min(progress + Time.deltaTime / duration, 1);
			
			transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
			transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, progress);

			yield return null;
		}

		collider.enabled = true;
	}

	public bool IsActive {
		set {
			foreach (WonderStage wonderStage in wonderStages) {
				wonderStage.IsActive = !wonderStage.IsBuilt && value;
			}
		}
	}

}
