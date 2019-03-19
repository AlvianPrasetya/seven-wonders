using UnityEngine;

public class RoomSlot : MonoBehaviour {

	public RoomPlayer roomPlayer;

	public bool IsOccupied() {
		return roomPlayer != null;
	}

	public void Occupy(RoomPlayer roomPlayer) {
		this.roomPlayer = roomPlayer;
		
		roomPlayer.transform.position = transform.position;
		roomPlayer.transform.rotation = transform.rotation;
	}

}
