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
	}

	public override void OnJoinedRoom() {
		RoomPlayer player = Instantiate(roomPlayerPrefab, transform.position, transform.rotation);
		player.Nickname = PhotonNetwork.LocalPlayer.NickName;
		player.IsReady = PhotonNetwork.LocalPlayer.IsMasterClient;

		playersByActorID.Add(PhotonNetwork.LocalPlayer.ActorNumber, player);
		
		foreach (RoomSlot slot in slots) {
			if (!slot.IsOccupied()) {
				slot.Occupy(player);
				break;
			}
		}
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.PhotonPlayer newPlayer) {
		RoomPlayer player = Instantiate(roomPlayerPrefab, transform.position, transform.rotation);
		player.Nickname = newPlayer.NickName;
		player.IsReady = newPlayer.IsMasterClient;

		playersByActorID.Add(newPlayer.ActorNumber, player);

		foreach (RoomSlot slot in slots) {
			if (!slot.IsOccupied()) {
				slot.Occupy(player);
				break;
			}
		}
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.PhotonPlayer otherPlayer) {
		Destroy(playersByActorID[otherPlayer.ActorNumber].gameObject);
		
		playersByActorID.Remove(otherPlayer.ActorNumber);
	}

	public void ToggleReady() {
		toggleReadyText.text = UnreadyText;

		photonView.RPC("ToggleReady", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
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
