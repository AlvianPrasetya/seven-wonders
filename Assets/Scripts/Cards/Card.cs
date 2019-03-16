using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour {

	private const float DefaultTranslateSpeed = 50.0f;
	private const float DefaultRotateSpeed = 60.0f;

	private new Rigidbody rigidbody;

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	public IEnumerator MoveTowards(
		Vector3 targetPosition, Quaternion targetRotation,
		float translateSpeed = DefaultTranslateSpeed, float rotateSpeed = DefaultRotateSpeed
	) {
		rigidbody.detectCollisions = false;
		rigidbody.useGravity = false;

		float sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
		while (sqrDistance > Constant.DistanceEpsilon * Constant.DistanceEpsilon) {
			float deltaDistance = translateSpeed * Time.deltaTime;
			Vector3 deltaPosition = (targetPosition - transform.position).normalized * deltaDistance;

			if (deltaDistance * deltaDistance > sqrDistance) {
				// Overshot, set position to target position
				transform.position = targetPosition;
			} else {
				transform.position += deltaPosition;
			}
			
			sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
			yield return null;
		}

		rigidbody.detectCollisions = true;
		rigidbody.useGravity = true;
	}

}
