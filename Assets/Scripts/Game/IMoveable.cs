using System.Collections;
using UnityEngine;

public interface IMoveable {
	
	IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation);

}
