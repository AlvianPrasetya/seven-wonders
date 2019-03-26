﻿using System.Collections;
using UnityEngine;

public class GameCamera : MonoBehaviour {

	public Vector3 selfFocusOffset = new Vector3(0, 25, -7);
	public float selfFocusAngle = 75;
	public Vector3 othersFocusOffset = new Vector3(0, 25, 0);
	public float othersFocusAngle = 75;
	public float refocusDuration = 1;

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
		Vector3 targetPosition;
		Quaternion targetRotation;
		if (player == GameManager.Instance.Player) {
			targetPosition = player.transform.position +
				player.transform.right * selfFocusOffset.x +
				player.transform.up * selfFocusOffset.y +
				player.transform.forward * selfFocusOffset.z;
			targetRotation = Quaternion.Euler(selfFocusAngle, player.transform.rotation.eulerAngles.y, 0);
		} else {	
			targetPosition = player.transform.position +
				player.transform.right * othersFocusOffset.x +
				player.transform.up * othersFocusOffset.y +
				player.transform.forward * othersFocusOffset.z;
			targetRotation = Quaternion.Euler(othersFocusAngle, player.transform.rotation.eulerAngles.y, 0);
		}

		yield return MoveTowards(targetPosition, targetRotation, refocusDuration);
	}

}
