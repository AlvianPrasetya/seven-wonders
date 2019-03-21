using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks {

	private const string StartText = "Start";
	private const string StartingText = "Starting";

	public Toggle readyToggle;
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
			readyToggle.gameObject.SetActive(false);

			startGameButton.gameObject.SetActive(true);
			startGameButton.interactable = false;
			startGameText.text = StartText;
		} else {
			readyToggle.gameObject.SetActive(true);

			startGameButton.gameObject.SetActive(false);
		}

		// Sync the states of all existing players through their custom properties
		foreach (Photon.Realtime.PhotonPlayer photonPlayer in PhotonNetwork.PlayerList) {
			RoomPlayer player = CreatePlayer(photonPlayer);
			playersByActorID.Add(photonPlayer.ActorNumber, player);
			OnPlayerPropertiesUpdate(photonPlayer, photonPlayer.CustomProperties);
		}
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.PhotonPlayer newPlayer) {
		RoomPlayer player = CreatePlayer(newPlayer);

		playersByActorID.Add(newPlayer.ActorNumber, player);
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.PhotonPlayer otherPlayer) {
		RoomPlayer player = playersByActorID[otherPlayer.ActorNumber];
		player.Unoccupy();
		Destroy(player.gameObject);

		playersByActorID.Remove(otherPlayer.ActorNumber);
	}

	public override void OnPlayerPropertiesUpdate(
		Photon.Realtime.PhotonPlayer target, ExitGames.Client.Photon.Hashtable changedProps
	) {
		RoomPlayer player = playersByActorID[target.ActorNumber];

		object ready;
		player.IsReady = changedProps.TryGetValue(PlayerProperty.Ready, out ready) && (bool)ready;

		object pos;
		if (changedProps.TryGetValue(PlayerProperty.Pos, out pos)) {
			player.Occupy(slots[(int)pos]);
		}

		if (PhotonNetwork.IsMasterClient) {
			startGameButton.interactable = AreAllPlayersReady();
		}
	}

	public override void OnMasterClientSwitched(Photon.Realtime.PhotonPlayer newMasterClient) {
		// TODO: Implement master client switch logic
	}

	public void SetReady(bool ready) {
		photonView.RPC("SetReady", RpcTarget.MasterClient, ready);
	}

	public void SetPos(int pos) {
		photonView.RPC("SetPos", RpcTarget.MasterClient, pos);
	}

	public void StartGame() {
		startGameText.text = StartingText;
		startGameButton.interactable = false;

		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;

		// Set match seed
		SetMatchSeed(new System.Random().Next());

		PhotonNetwork.LoadLevel(LevelName.Game);
	}

	[PunRPC]
	private void SetReady(bool ready, PhotonMessageInfo info) {
		Photon.Realtime.PhotonPlayer sender = info.Sender;
		SetPlayerReady(sender, ready);
	}

	[PunRPC]
	private void SetPos(int pos, PhotonMessageInfo info) {
		if (slots[pos].IsOccupied) {
			// Already occupied
			return;
		}
		Photon.Realtime.PhotonPlayer sender = info.Sender;
		SetPlayerPos(sender, pos);
	}

	private void SetPlayerReady(Photon.Realtime.PhotonPlayer player, bool ready) {
		ExitGames.Client.Photon.Hashtable customProperties = player.CustomProperties;
		customProperties[PlayerProperty.Ready] = ready;
		player.SetCustomProperties(customProperties);
	}

	private void SetPlayerPos(Photon.Realtime.PhotonPlayer player, int pos) {
		ExitGames.Client.Photon.Hashtable customProperties = player.CustomProperties;
		customProperties[PlayerProperty.Pos] = pos;
		player.SetCustomProperties(customProperties);
	}

	private void SetMatchSeed(int matchSeed) {
		ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
		customProperties[RoomProperty.MatchSeed] = matchSeed;
		PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
	}

	private RoomPlayer CreatePlayer(Photon.Realtime.PhotonPlayer photonPlayer) {
		RoomPlayer player = Instantiate(roomPlayerPrefab, transform.position, transform.rotation);
		player.Nickname = photonPlayer.NickName;
		player.IsReady = false;

		if (PhotonNetwork.IsMasterClient) {
			SetPlayerReady(photonPlayer, photonPlayer.IsMasterClient);
			for (int i = 0; i < slots.Length; i++) {
				if (!slots[i].IsOccupied) {
					SetPlayerPos(photonPlayer, i);
					break;
				}
			}
		}

		return player;
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
