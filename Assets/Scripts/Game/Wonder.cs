using System.Collections;
using UnityEngine;

public class Wonder : MonoBehaviour {

	public PlayArea[] buryPlayAreas;
	public WonderStage[] wonderStages;

	public bool IsPlayable {
		set {
			foreach (PlayArea buryPlayArea in buryPlayAreas) {
				buryPlayArea.IsPlayable = value;
			}
		}
	}

}
