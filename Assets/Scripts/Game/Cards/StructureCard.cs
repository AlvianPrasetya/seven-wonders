using UnityEngine;
using UnityEngine.EventSystems;

public class StructureCard : Card {

	public Age age;

	public override void OnBeginDrag(PointerEventData eventData) {
		base.OnBeginDrag(eventData);

		GameManager.Instance.Player.EnableBuildArea(this);
		GameManager.Instance.Player.EnableBuryAreas(this);
		GameManager.Instance.Player.EnableDiscardArea();
	}

	public override void OnEndDrag(PointerEventData eventData) {
		base.OnEndDrag(eventData);

		GameManager.Instance.Player.DisableBuildArea();
		GameManager.Instance.Player.DisableBuryAreas();
		GameManager.Instance.Player.DisableDiscardArea();
	}

}
