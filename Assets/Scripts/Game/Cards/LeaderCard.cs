using UnityEngine;
using UnityEngine.EventSystems;

public class LeaderCard : Card {

	public bool drafted;

	protected override void Awake() {
		base.Awake();
		
		drafted = false;
	}

	public override void OnBeginDrag(PointerEventData eventData) {
		base.OnBeginDrag(eventData);

		if (!drafted) {
			GameManager.Instance.Player.EnableDraftArea();
		} else {
			GameManager.Instance.Player.EnableBuildArea(this);
			GameManager.Instance.Player.EnableBuryAreas(this);
			GameManager.Instance.Player.EnableDiscardArea();
		}
	}

	public override void OnEndDrag(PointerEventData eventData) {
		base.OnEndDrag(eventData);
		
		if (!drafted) {
			GameManager.Instance.Player.DisableDraftArea();
		} else {
			GameManager.Instance.Player.DisableBuildArea();
			GameManager.Instance.Player.DisableBuryAreas();
			GameManager.Instance.Player.DisableDiscardArea();
		}
	}

}
