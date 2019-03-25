using UnityEngine;
using UnityEngine.EventSystems;

public class StructureCard : Card {

	public int minPlayers;
	public Age age;

	public override void OnEndDrag(PointerEventData eventData) {
		base.OnEndDrag(eventData);

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(LayerName.DropArea))) {
			DropArea<Card> cardDropArea = hitInfo.transform.GetComponent<DropArea<Card>>();
			cardDropArea.Drop(this);
		} else {
			transform.position = dragStartPosition;
		}
	}

}
