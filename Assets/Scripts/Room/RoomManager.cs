using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks {

	private const string ReadyText = "Ready";
	private const string UnreadyText = "Unready";
	private const string StartText = "Start";
	private const string StartingText = "Starting";

	public Button toggleReadyButton;
	public Text toggleReadyText;
	public Button startGameButton;
	public Text startGameText;
	public RoomPlayer roomPlayerPrefab;
	public RoomSlot[] slots;

	private Dictionary<int, RoomPlayer> playersByActorID;

	void Awake() {
		playersByActorID = new Dictionary<int, RoomPlayer>();

		PhotonNetwork.AutomaticallySyncScene = true;
	}

	void Start() {
		if (PhotonNetwork.IsMasterClient) {
			toggleReadyButton.gameObject.SetActive(false);

			startGameButton.gameObject.SetActive(true);
			startGameButton.interactable = false;
			startGameText.text = StartText;
		} else {
			toggleReadyButton.gameObject.SetActive(true);
			toggleReadyText.text = ReadyText;

			startGameButton.gameObject.SetActive(false);
		}

		foreach (Photon.Realtime.PhotonPlayer player in PhotonNetwork.PlayerListOthers) {
			CreatePlayer(player);
		}
		CreatePlayer(PhotonNetwork.LocalPlayer);
		if (PhotonNetwork.IsMasterClient) {
			// Master client is immediately ready when joining the room
			ToggleReady();
		} else {
			// Other clients are not ready when joining the room
			ToggleReady();
			ToggleReady();
		}
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.PhotonPlayer newPlayer) {
		CreatePlayer(newPlayer);
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.PhotonPlayer otherPlayer) {
		Destroy(playersByActorID[otherPlayer.ActorNumber].gameObject);
		
		playersByActorID.Remove(otherPlayer.ActorNumber);
	}

	public void ToggleReady() {
		RoomPlayer player = playersByActorID[PhotonNetwork.LocalPlayer.ActorNumber];
		toggleReadyText.text = player.IsReady ? ReadyText : UnreadyText;

		photonView.RPC("ToggleReady", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
	}

	public void StartGame() {
		startGameText.text = StartingText;
		startGameButton.interactable = false;

		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;
		PhotonNetwork.LoadLevel(LevelName.Game);
	}

	[PunRPC]
	void ToggleReady(int actorID) {
		// Set ready property
		RoomPlayer player;
		if (playersByActorID.TryGetValue(actorID, out player)) {
			player.IsReady = !player.IsReady;
		}

		if (PhotonNetwork.IsMasterClient) {
			startGameButton.interactable = AreAllPlayersReady();
		}
	}

	private void CreatePlayer(Photon.Realtime.PhotonPlayer photonPlayer) {
		RoomPlayer player = Instantiate(roomPlayerPrefab, transform.position, transform.rotation);
		player.Nickname = photonPlayer.NickName;
		player.IsReady = false;

		playersByActorID.Add(photonPlayer.ActorNumber, player);

		foreach (RoomSlot slot in slots) {
			if (!slot.IsOccupied()) {
				slot.Occupy(player);
				break;
			}
		}
	}

	private bool AreAllPlayersReady() {
		bool allReady = true;
		foreach (KeyValuePair<int, RoomPlayer> entry in playersByActorID) {
			RoomPlayer player = entry.Value;
			if (!player.IsReady) {
				allReady = false;
				break;
			}
		}

		return allReady;
	}

}
