using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour, IMoveable {

	private const float TranslateSpeed = 150;
	private const float RotateSpeed = 1440;

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

}
