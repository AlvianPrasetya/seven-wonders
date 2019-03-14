using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour {

	private const float TranslateSpeed = 100.0f;
	private const float RotateSpeed = 60.0f;

	public IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation) {
		float sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
		while (sqrDistance > Constant.DistanceEpsilon * Constant.DistanceEpsilon) {
			float deltaDistance = TranslateSpeed * Time.deltaTime;
			Vector3 deltaPosition = (targetPosition - transform.position).normalized * deltaDistance;

			if (deltaDistance * deltaDistance > sqrDistance) {
				// Overshot, set position to target position
				transform.position = targetPosition;
				yield break;
			}

			transform.position += deltaPosition;
			
			sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
			yield return null;
		}
	}

}
