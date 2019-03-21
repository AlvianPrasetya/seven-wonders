using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, IMoveable, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public float dragHeight = 2;

	protected new Collider collider;
	protected new Rigidbody rigidbody;
	protected Vector3 dragStartPosition;

	void Awake() {
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public IEnumerator MoveTowards(Vector3 targetPosition, Quaternion targetRotation, float duration) {
		collider.enabled = false;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;

		Vector3 initialPosition = transform.position;
		Quaternion initialRotation = transform.rotation;

		float progress = 0;
		while (progress < 1) {
			progress = Mathf.Min(progress + Time.deltaTime / duration, 1);
			
			transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
			transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, progress);

			yield return null;
		}

		collider.enabled = true;
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
	}

	public virtual void OnBeginDrag(PointerEventData eventData) {
		collider.enabled = false;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;

		dragStartPosition = transform.position;
	}

	public virtual void OnDrag(PointerEventData eventData) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.Table))) {
			transform.position = hitInfo.point - dragHeight * ray.direction;
		}
	}

	public virtual void OnEndDrag(PointerEventData eventData) {
		collider.enabled = true;
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
	}

}
