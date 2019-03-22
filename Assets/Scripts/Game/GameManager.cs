using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun {

	private const string MatchSeedKey = "match_seed";

	[System.Serializable]
	public class StockEntry {

		public StockType stockType;
		public Stock stock;

	}

	public GameCamera gameCamera;
	public Bank bank;
	public Deck discardPile;
	public StockEntry[] stocks;
	public Player playerPrefab;

	public static GameManager Instance { get; private set; }
	public Dictionary<StockType, Stock> Stocks { get; private set; }
	public List<Player> Players { get; private set; }
	public Dictionary<int, Player> PlayersByActorID { get; private set; }
	public Player Player { get; private set; }

	private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;
		Stocks = new Dictionary<StockType, Stock>();
		Players = new List<Player>();
		PlayersByActorID = new Dictionary<int, Player>();
		foreach (StockEntry stockEntry in stocks) {
			Stocks.Add(stockEntry.stockType, stockEntry.stock);
		}
		resolverQueue = new ResolverQueue();
	}

	void Start() {
		// Create player at location determined by player pos
		Player[] playersByPos = new Player[7];
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
			int pos = (int)PhotonNetwork.PlayerList[i].CustomProperties[PlayerProperty.Pos];
			float playerAngle = pos * 360f / 7;
			Vector3 playerPosition = new Vector3(
				Mathf.Sin(Mathf.Deg2Rad * playerAngle) * 35,
				0,
				-Mathf.Cos(Mathf.Deg2Rad * playerAngle) * 35
			);
			Quaternion playerRotation = Quaternion.Euler(0, -playerAngle, 0);
			
			Player player = Instantiate(playerPrefab, playerPosition, playerRotation);
			playersByPos[pos] = player;
			if (PhotonNetwork.PlayerList[i].IsLocal) {
				Player = player;
			}
		}

		// Store players in a compact list ordered by pos
		for (int i = 0; i < playersByPos.Length; i++) {
			if (playersByPos[i] != null) {
				Players.Add(playersByPos[i]);
			}
		}

		// Assign neighbours
		for (int i = 0; i < Players.Count; i++) {
			Players[i].westNeighbour = Players[(Players.Count + i - 1) % Players.Count];
			Players[i].eastNeighbour = Players[(i + 1) % Players.Count];
		}

		resolverQueue.Enqueue(
			new MatchResolver((int)PhotonNetwork.CurrentRoom.CustomProperties[MatchSeedKey]),
			1
		);
		StartCoroutine(Resolve());
	}

	public void EnqueueResolver(IResolvable resolver, int priority) {
		resolverQueue.Enqueue(resolver, priority);
	}

	public void PlayBuild(int positionInHand) {
		photonView.RPC("PlayBuild", RpcTarget.All, positionInHand);
	}

	public void PlayBury(int positionInHand, int wonderStage) {
		photonView.RPC("PlayBury", RpcTarget.All, positionInHand, wonderStage);
	}

	public void PlayDiscard(int positionInHand) {
		photonView.RPC("PlayDiscard", RpcTarget.All, positionInHand);
	}

	[PunRPC]
	private void PlayBuild(int positionInHand, PhotonMessageInfo info) {
		Player player = PlayersByActorID[info.Sender.ActorNumber];
		StartCoroutine(player.PrepareBuild(positionInHand));
	}

	[PunRPC]
	private void PlayBury(int positionInHand, int wonderStage, PhotonMessageInfo info) {
		Player player = PlayersByActorID[info.Sender.ActorNumber];
		StartCoroutine(player.PrepareBury(positionInHand, wonderStage));
	}

	[PunRPC]
	private void PlayDiscard(int positionInHand, PhotonMessageInfo info) {
		Player player = PlayersByActorID[info.Sender.ActorNumber];
		StartCoroutine(player.PrepareDiscard(positionInHand));
	}

	private IEnumerator Resolve() {
		while (resolverQueue.Size() != 0) {
			yield return resolverQueue.Dequeue().Resolve();
			Debug.Log("Queue size: " + resolverQueue.Size());
		}
	}

}