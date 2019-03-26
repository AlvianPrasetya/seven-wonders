using System.Collections;
using UnityEngine;

public class WonderStage : MonoBehaviour {

	public BuryDropArea buryDropArea;
	public CardSlot buildCardSlot;
	public OnBuildEffect[] onBuildEffects;
	public bool IsBuilt {
		get {
			return buildCardSlot.Element != null;
		}
	}
	public bool IsPlayable {
		set {
			buryDropArea.IsPlayable = value;
		}
	}
	
}
