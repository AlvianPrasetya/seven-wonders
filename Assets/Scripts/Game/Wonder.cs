using System.Collections;
using UnityEngine;

public class Wonder : MonoBehaviour {

	public BuryDropArea[] buryDropAreas;
	public WonderStage[] wonderStages;

	public bool IsPlayable {
		set {
			foreach (BuryDropArea buryDropArea in buryDropAreas) {
				buryDropArea.IsActive = value;
			}
		}
	}

}
