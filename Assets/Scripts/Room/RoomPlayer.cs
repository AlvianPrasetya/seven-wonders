using Photon.Pun;
using UnityEngine;

public class RoomPlayer : MonoBehaviour {

	public TextMesh nicknameMesh;
	public MeshRenderer readyMesh;
	public RoomSlot roomSlot;

	public string Nickname {
		get {
			return nicknameMesh.text;
		}

		set {
			nicknameMesh.text = value;
		}
	}

	public bool IsReady {
		get {
			return readyMesh.gameObject.activeSelf;
		}

		set {
			readyMesh.gameObject.SetActive(value);
		}
	}

	public void Occupy(RoomSlot newRoomSlot) {
		RoomSlot prevRoomSlot = roomSlot;
		if (prevRoomSlot != null) {
			prevRoomSlot.IsOccupied = false;
		}

		newRoomSlot.IsOccupied = true;
		roomSlot = newRoomSlot;
		
		transform.position = newRoomSlot.transform.position;
		transform.rotation = newRoomSlot.transform.rotation;
	}

	public void Unoccupy() {
		if (roomSlot != null) {
			roomSlot.IsOccupied = false;
		}

		roomSlot = null;
	}

}
