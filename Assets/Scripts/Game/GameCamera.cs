using System.Collections;
using UnityEngine;

public class GameCamera : MonoBehaviour {

	public float PlayerFocusDistance = 30;
	public float PlayerFocusAngle = 60;

	public IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation, float duration) {
		Vector3 initialPosition = transform.position;
		Quaternion initialRotation = transform.rotation;

		float progress = 0;
		while (progress < 1) {
			progress = Mathf.Min(progress + Time.deltaTime / duration, 1);
			
			transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
			transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, progress);

			yield return null;
		}
	}

	/// <summary>
	/// Focuses the camera on the specified player.
	/// </summary>
	public IEnumerator Focus(Player player) {
		yield return MoveTowards(
			player.transform.position +
				player.transform.up * Mathf.Sin(Mathf.Deg2Rad * PlayerFocusAngle) * PlayerFocusDistance +
				player.transform.forward * -Mathf.Cos(Mathf.Deg2Rad * PlayerFocusAngle) * PlayerFocusDistance,
			Quaternion.Euler(PlayerFocusAngle, player.transform.rotation.eulerAngles.y, 0),
			1
		);
	}

}
