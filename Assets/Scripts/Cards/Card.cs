using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour, IMoveable {

	private const float TranslateSpeed = 200;
	private const float RotateSpeed = 1440;

	private new Collider collider;
	private new Rigidbody rigidbody;

	void Awake() {
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public IEnumerator MoveTowards(
		Vector3 targetPosition, Quaternion targetRotation
	) {
		collider.enabled = false;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;

		float sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
		float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);
		while (sqrDistance > Constant.DistanceEpsilon * Constant.DistanceEpsilon || angleDiff > Constant.AngleEpsilon) {
			float deltaDistance = TranslateSpeed * Time.deltaTime;
			Vector3 deltaPosition = (targetPosition - transform.position).normalized * deltaDistance;
			if (deltaDistance * deltaDistance > sqrDistance) {
				// Overshot, set position to target position
				transform.position = targetPosition;
			} else {
				transform.position += deltaPosition;
			}
			sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);

			float deltaAngle = RotateSpeed * Time.deltaTime;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, deltaAngle);
			angleDiff = Quaternion.Angle(transform.rotation, targetRotation);

			yield return null;
		}

		collider.enabled = true;
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
	}

}
