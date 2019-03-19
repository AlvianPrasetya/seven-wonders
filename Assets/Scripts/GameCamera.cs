using System.Collections;
using UnityEngine;

public class GameCamera : MonoBehaviour {

	public IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation, float duration) {
		float totalSpatialDistance = Vector3.Distance(transform.position, targetPosition);
		float totalAngularDistance = Quaternion.Angle(transform.rotation, targetRotation);
		float translateSpeed = totalSpatialDistance / duration;
		float rotateSpeed = totalAngularDistance / duration;

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
	}

}
