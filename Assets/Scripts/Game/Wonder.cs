using System.Collections;
using UnityEngine;

public class Wonder : MonoBehaviour, IMoveable {

	public BuryDropArea[] buryDropAreas;
	public WonderStage[] wonderStages;

	protected new Collider collider;
	protected new Rigidbody rigidbody;

	void Awake() {
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation, float duration) {
		collider.enabled = false;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;

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
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
	}

	public bool IsPlayable {
		set {
			for (int i = 0; i < buryDropAreas.Length; i++) {
				if (wonderStages[i].buildCardSlot.Element == null) {
					buryDropAreas[i].IsActive = value;
				}
			}
		}
	}

}
