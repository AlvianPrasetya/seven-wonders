using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour {

	private const float DefaultTranslateSpeed = 50.0f;
	private const float DefaultRotateSpeed = 60.0f;

	private new Collider collider;
	private new Rigidbody rigidbody;

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

		Vector3 initialPosition = transform.position;

		float dist = Vector3.Distance(initialPosition, targetPosition);
		float distY = Mathf.Abs(initialPosition.y - targetPosition.y);
		float arcRadius = dist / (2 * Mathf.Cos(Mathf.Asin(distY / dist)));

		if (initialPosition.y < targetPosition.y) {
			Vector3 arcPlanarDirection = new Vector3(
				targetPosition.x - initialPosition.x,
				0,
				targetPosition.z - initialPosition.z
			).normalized;
			Vector3 arcCenter = initialPosition + arcPlanarDirection * arcRadius;
			
			float currentTrajectoryAngle = -Mathf.PI;
			float sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
			while (sqrDistance > Constant.DistanceEpsilon * Constant.DistanceEpsilon) {
				float deltaDistance = translateSpeed * Time.deltaTime;
				if (deltaDistance * deltaDistance > sqrDistance) {
					// Overshot, set position to target position
					transform.position = targetPosition;
				} else {
					float deltaAngle = deltaDistance / arcRadius;
					currentTrajectoryAngle += deltaAngle;
					// Calculate the delta from center of arc circle
					float deltaXZ = Mathf.Cos(currentTrajectoryAngle) * arcRadius;
					float deltaY = Mathf.Sin(currentTrajectoryAngle) * arcRadius;
					transform.position = arcCenter - arcPlanarDirection * deltaXZ + Vector3.up * deltaY;
				}
				sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);

				yield return null;
			}
		} else {
			Vector3 arcPlanarDirection = new Vector3(
					initialPosition.x - targetPosition.x,
					0,
					initialPosition.z - targetPosition.z
			).normalized;
			Vector3 arcCenter = targetPosition + arcPlanarDirection * arcRadius;

			float currentTrajectoryAngle = Mathf.Asin(distY / arcRadius);
			float sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
			while (sqrDistance > Constant.DistanceEpsilon * Constant.DistanceEpsilon) {
				float deltaDistance = translateSpeed * Time.deltaTime;
				if (deltaDistance * deltaDistance > sqrDistance) {
					// Overshot, set position to target position
					transform.position = targetPosition;
				} else {
					float deltaAngle = deltaDistance / arcRadius;
					currentTrajectoryAngle -= deltaAngle;
					// Calculate the delta from center of arc circle
					float deltaXZ = Mathf.Cos(currentTrajectoryAngle) * arcRadius;
					float deltaY = Mathf.Sin(currentTrajectoryAngle) * arcRadius;
					transform.position = arcCenter - arcPlanarDirection * deltaXZ + Vector3.up * deltaY;
				}
				sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);

				yield return null;
			}
		}

		collider.enabled = true;
		rigidbody.useGravity = true;
	}

}
