using UnityEngine;

public class Board : MonoBehaviour {

	[System.Serializable]
	public struct WonderPosition {
		// public Wonder wonder;
		public Vector2 relativePosition;
	}

	public WonderPosition[] wonderPositions;

}
