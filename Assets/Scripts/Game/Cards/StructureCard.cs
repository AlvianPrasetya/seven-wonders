using UnityEngine;
using UnityEngine.EventSystems;

public class StructureCard : Card, IBuildable, IBuriable, IDiscardable {

	public int minPlayers;
	public Age age;

	public override void OnEndDrag(PointerEventData eventData) {
		base.OnEndDrag(eventData);

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.PlayArea))) {
			PlayArea playArea = hitInfo.transform.GetComponent<PlayArea>();
			playArea.Play(this);
		} else {
			transform.position = dragStartPosition;
		}
	}
	
	public void Build() {
	}

	public void Bury() {
	}

	public void Discard() {
	}

}