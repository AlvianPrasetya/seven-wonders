using System.Collections;
using UnityEngine;

public class MilitaryToken : MonoBehaviour, IMoveable {
	
	public int points;
	public MilitaryTokenType type;

	public IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation, float duration) {
		Vector3 initialPosition = transform.position;
		Quaternion initialRotation = transform.rotation;
		float initialDistance = Vector3.Distance(initialPosition, targetPosition);
		float initialDistanceY = Mathf.Abs(initialPosition.y - targetPosition.y);

		float sphereRadius = 0.5f * initialDistance / Mathf.Cos(Mathf.Asin(initialDistanceY / initialDistance));
		Vector3 sphereCenter;
		if (initialPosition.y < targetPosition.y) {
			sphereCenter = initialPosition + new Vector3(
				targetPosition.x - initialPosition.x,
				0,
				targetPosition.z - initialPosition.z
			).normalized * sphereRadius;
		} else {
			sphereCenter = targetPosition + new Vector3(
				initialPosition.x - targetPosition.x,
				0,
				initialPosition.z - targetPosition.z
			).normalized * sphereRadius;
		}

		Vector3 initialPositionXZ = new Vector3(initialPosition.x, 0, initialPosition.z);
		Vector3 targetPositionXZ = new Vector3(targetPosition.x, 0, targetPosition.z);
		float progress = 0;
		while (progress < 1) {
			progress = Mathf.Min(progress + Time.deltaTime / duration, 1);

			Vector3 position = Vector3.Lerp(initialPositionXZ, targetPositionXZ, progress);
			// Sphere equation: (x - a)^2 + (y - b)^2 + (z - c)^2 = r^2
			position.y = Mathf.Sqrt(
				Mathf.Max(
					Mathf.Pow(sphereRadius, 2) - 
					Mathf.Pow(position.x - sphereCenter.x, 2) - 
					Mathf.Pow(position.z - sphereCenter.z, 2)
				, 0)
			) + sphereCenter.y;
			transform.position = position;
			transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, progress);

			yield return null;
		}
	}

}
