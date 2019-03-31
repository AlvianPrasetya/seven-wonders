using System.Collections;
using UnityEngine;

public class Wonder : MonoBehaviour, IMoveable {
	
	public Bank bank;
	public CardSlot preparedCardSlot;
	public WonderStage[] wonderStages;
	public OnBuildEffect[] onBuildEffects;

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

	public virtual WonderStage[] GetBuildableStages() {
		foreach (WonderStage wonderStage in wonderStages) {
			if (!wonderStage.IsBuilt) {
				return new WonderStage[]{ wonderStage };
			}
		}

		return new WonderStage[0];
	}

}
