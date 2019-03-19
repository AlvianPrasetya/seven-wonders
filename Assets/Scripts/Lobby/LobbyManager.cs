using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks {

	private const float BackgroundStayDuration = 5;
	private const float BackgroundTransitionDuration = 2;

	public Image backgroundImageFrontBuffer;
	public Image backgroundImageBackBuffer;
	public InputField nicknameInputField;
	public Text statusText;
	public Sprite[] backgroundSprites;

	void Start() {
		backgroundImageFrontBuffer.sprite = backgroundSprites[0];
		StartCoroutine(TransitionBackground());
	}

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

		PhotonNetwork.LoadLevel(LevelName.Room);
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

	private IEnumerator TransitionBackground() {
		int backgroundIndex = 0;
		while (true) {
			yield return new WaitForSeconds(BackgroundStayDuration);

			backgroundImageBackBuffer.sprite = backgroundSprites[++backgroundIndex % backgroundSprites.Length];
			float fadeToBackBufferProgress = 0;
			while (fadeToBackBufferProgress <= 1) {
				backgroundImageFrontBuffer.color = new Color(
					backgroundImageFrontBuffer.color.r,
					backgroundImageFrontBuffer.color.g,
					backgroundImageFrontBuffer.color.b,
					Mathf.Lerp(1, 0, fadeToBackBufferProgress)
				);

				backgroundImageBackBuffer.color = new Color(
					backgroundImageBackBuffer.color.r,
					backgroundImageBackBuffer.color.g,
					backgroundImageBackBuffer.color.b,
					Mathf.Lerp(0, 1, fadeToBackBufferProgress)
				);

				fadeToBackBufferProgress += Time.deltaTime / BackgroundTransitionDuration;
				yield return null;
			}

			yield return new WaitForSeconds(BackgroundStayDuration);

			backgroundImageFrontBuffer.sprite = backgroundSprites[++backgroundIndex % backgroundSprites.Length];
			float fadeToFrontBufferProgress = 0;
			while (fadeToFrontBufferProgress <= 1) {
				backgroundImageFrontBuffer.color = new Color(
					backgroundImageFrontBuffer.color.r,
					backgroundImageFrontBuffer.color.g,
					backgroundImageFrontBuffer.color.b,
					Mathf.Lerp(0, 1, fadeToFrontBufferProgress)
				);

				backgroundImageBackBuffer.color = new Color(
					backgroundImageBackBuffer.color.r,
					backgroundImageBackBuffer.color.g,
					backgroundImageBackBuffer.color.b,
					Mathf.Lerp(1, 0, fadeToFrontBufferProgress)
				);

				fadeToFrontBufferProgress += Time.deltaTime / BackgroundTransitionDuration;
				yield return null;
			}
		}
	}

}
