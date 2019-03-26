using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, IMoveable, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public string cardName;
	public CardType cardType;
	public DisplayType displayType;
	public float dragHeight = 2;
	public OnBuildEffect[] onBuildEffects;
	
	protected Vector3 dragStartPosition;

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

	public void OnBeginDrag(PointerEventData eventData) {
		dragStartPosition = transform.position;
	}

	public void OnDrag(PointerEventData eventData) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.Table))) {
			transform.position = hitInfo.point - dragHeight * ray.direction;
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.DropArea))) {
			DropArea<Card> cardDropArea = hitInfo.transform.GetComponent<DropArea<Card>>();
			cardDropArea.Drop(this);
		} else {
			transform.position = dragStartPosition;
		}
	}

	public IEnumerator Flip() {
		Vector3 targetEulerAngles = transform.eulerAngles + new Vector3(0, 0, 180);
		Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
		yield return MoveTowards(transform.position, targetRotation, 0.2f);
	}

}
