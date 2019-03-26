using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SwipeSurface : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[System.Serializable]
	public class OnSwipedLeftEvent : UnityEvent {}
	
	[System.Serializable]
	public class OnSwipedRightEvent : UnityEvent {}

	public float swipeDistanceThreshold = 50;
	public float swipeAngleThreshold = 45;
	public OnSwipedLeftEvent onSwipedLeftEvent;
	public OnSwipedRightEvent onSwipedRightEvent;

	private new Collider collider;

	void Awake() {
		collider = GetComponent<Collider>();
	}

	public bool IsActive {
		get {
			return collider.enabled;
		}

		set {
			collider.enabled = value;
		}
	}

	public void OnBeginDrag(PointerEventData eventData) {
	}

	public void OnDrag(PointerEventData eventData) {
	}

	public void OnEndDrag(PointerEventData eventData) {
		float sqrDist = Vector2.SqrMagnitude(eventData.position - eventData.pressPosition);
		if (sqrDist > swipeDistanceThreshold * swipeDistanceThreshold) {
			float angleFromLeft = Vector2.Angle(
				eventData.position - eventData.pressPosition,
				Vector2.left
			);
			float angleFromRight = Vector2.Angle(
				eventData.position - eventData.pressPosition,
				Vector2.right
			);

			if (angleFromLeft < swipeAngleThreshold) {
				// Left swipe
				onSwipedLeftEvent.Invoke();
			} else if (angleFromRight < swipeAngleThreshold) {
				// Right swipe
				onSwipedRightEvent.Invoke();
			}
		}
	}

}
