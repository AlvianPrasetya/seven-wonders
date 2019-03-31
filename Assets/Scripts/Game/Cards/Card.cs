using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, IMoveable, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public string cardName;
	public CardType cardType;
	public DisplayType displayType;
	public float dragHeight = 2;
	public int coinCost;
	public Resource[] resourceCost;
	public OnBuildEffect[] onBuildEffects;
	
	private bool dragged;
	private Vector3 dragStartPosition;
	private new Collider collider;
	private DropArea<Card> lastDropArea;

	void Awake() {
		collider = GetComponent<Collider>();
	}

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
		dragged = true;
		dragStartPosition = transform.position;
		GameManager.Instance.Player.EnableBuildAreas(this);
	}

	public void OnDrag(PointerEventData eventData) {
		if (!dragged) {
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.Table))) {
			transform.position = hitInfo.point - dragHeight * ray.direction;
		}

		if (lastDropArea != null) {
			lastDropArea.IsHighlighted = false;
			lastDropArea = null;
		}
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.DropArea))) {
			lastDropArea = hitInfo.transform.GetComponent<DropArea<Card>>();
			lastDropArea.IsHighlighted = true;
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (!dragged) {
			return;
		}
		
		if (lastDropArea) {
			lastDropArea.Drop(this);
			lastDropArea.IsHighlighted = false;
			lastDropArea = null;
		} else {
			transform.position = dragStartPosition;
		}

		dragged = false;
		GameManager.Instance.Player.DisableBuildAreas();
	}

	public bool IsPlayable {
		set {
			collider.enabled = value;

			if (!value && dragged) {
				// Forcefully return card to hand
				lastDropArea = null;
				OnEndDrag(null);
			}
		}
	}

	public IEnumerator Flip() {
		Vector3 targetEulerAngles = transform.eulerAngles + new Vector3(0, 0, 180);
		Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
		yield return MoveTowards(transform.position, targetRotation, 0.2f);
	}

}
