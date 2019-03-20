using UnityEngine;
using UnityEngine.EventSystems;

public class StructureCard : Card, IBuildable, IBuriable, IDiscardable, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public int minPlayers;
	public Age age;
	
	private Vector3 dragStartPosition;
	
	public void Build() {
	}

	public void Bury() {
	}

	public void Discard() {
	}

	public void OnBeginDrag(PointerEventData eventData) {
		collider.enabled = false;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;

		dragStartPosition = transform.position;
	}

	public void OnDrag(PointerEventData eventData) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.Table))) {
			transform.position = new Vector3(
				hitInfo.point.x,
				dragStartPosition.y,
				hitInfo.point.z
			);
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		collider.enabled = true;
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
	}

}
