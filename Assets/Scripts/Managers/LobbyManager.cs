using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks {

	public InputField nicknameInputField;
	public Text statusText;

	public override void OnConnectedToMaster() {
		string statusText = "Connected";
		Debug.Log(statusText);
		this.statusText.text = statusText;
		
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnDisconnected(Photon.Realtime.DisconnectCause cause) {
		string statusText = string.Format("Disconnected due to {0}", cause);
		Debug.Log(statusText);
		this.statusText.text = statusText;
	}

	public override void OnJoinedRoom() {
		Debug.Log(string.Format("Joined room {0}", PhotonNetwork.CurrentRoom.Name));
		this.statusText.text = "Joined room";

		PhotonNetwork.LoadLevel(LevelName.Game);
		string statusText = "Loading room";
		Debug.Log(statusText);
		this.statusText.text = statusText;
	}

	public override void OnJoinRandomFailed(short returnCode, string message) {
		string statusText = "Creating a new room";
		Debug.Log(statusText);
		this.statusText.text = statusText;

		Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
		roomOptions.MaxPlayers = 7;

		PhotonNetwork.CreateRoom(null, roomOptions);
	}

	public void Play() {
		PhotonNetwork.NickName = nicknameInputField.text;
		Connect();
	}

	private void Connect() {
		string statusText = "Connecting";
		Debug.Log(statusText);
		this.statusText.text = statusText;

		PhotonNetwork.GameVersion = Constant.GameVersion;
		PhotonNetwork.ConnectUsingSettings();
	}

}
