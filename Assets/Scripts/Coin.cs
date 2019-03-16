using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour {

	private const float DefaultTranslateSpeed = 50.0f;
	private const float DefaultRotateSpeed = 60.0f;

	private Collider collider;
	private Rigidbody rigidbody;

	void Awake() {
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public IEnumerator MoveTowards(
		Vector3 targetPosition, Quaternion targetRotation,
		float translateSpeed = DefaultTranslateSpeed, float rotateSpeed = DefaultRotateSpeed
	) {
		collider.enabled = false;
		rigidbody.useGravity = false;

		float sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
		float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);
		while (sqrDistance > Constant.DistanceEpsilon * Constant.DistanceEpsilon || angleDiff > Constant.AngleEpsilon) {
			float deltaDistance = translateSpeed * Time.deltaTime;
			Vector3 deltaPosition = (targetPosition - transform.position).normalized * deltaDistance;
			if (deltaDistance * deltaDistance > sqrDistance) {
				// Overshot, set position to target position
				transform.position = targetPosition;
			} else {
				transform.position += deltaPosition;
			}
			sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);

			float deltaAngle = rotateSpeed * Time.deltaTime;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, deltaAngle);
			angleDiff = Quaternion.Angle(transform.rotation, targetRotation);

			yield return null;
		}

		collider.enabled = true;
		rigidbody.useGravity = true;
	}

}
