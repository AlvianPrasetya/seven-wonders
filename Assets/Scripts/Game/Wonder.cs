using System.Collections;
using UnityEngine;

public class Wonder : MonoBehaviour {

	public BuryDropArea[] buryDropAreas;
	public WonderStage[] wonderStages;

	public bool IsPlayable {
		set {
			for (int i = 0; i < buryDropAreas.Length; i++) {
				if (wonderStages[i].buildCardSlot.Element == null) {
					buryDropAreas[i].IsActive = value;
				}
			}
		}
	}

}
