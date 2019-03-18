using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks {

	public InputField nickname;
	public Text status;

	void Awake() {
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public void Play() {
		PhotonNetwork.NickName = nickname.text;
		Connect();
	}

	private void Connect() {
		string statusText = "Connecting";
		Debug.Log(statusText);
		status.text = statusText;

		PhotonNetwork.GameVersion = Constant.GameVersion;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster() {
		string statusText = "Connected";
		Debug.Log(statusText);
		status.text = statusText;
		
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnDisconnected(Photon.Realtime.DisconnectCause cause) {
		string statusText = string.Format("Disconnected due to {0}", cause);
		Debug.Log(statusText);
		status.text = statusText;
	}

	public override void OnJoinedRoom() {
		Debug.Log(string.Format("Joined room {0}", PhotonNetwork.CurrentRoom.Name));
		status.text = "Joined room";

		PhotonNetwork.LoadLevel(LevelName.Room);
	}

	public override void OnJoinRandomFailed(short returnCode, string message) {
		string statusText = "Creating a new room";
		Debug.Log(statusText);
		status.text = statusText;

		Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
		roomOptions.MaxPlayers = 7;

		PhotonNetwork.CreateRoom(null, roomOptions);
	}

}
