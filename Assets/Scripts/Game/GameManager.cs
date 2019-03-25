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
	public WonderStock wonderStock;
	public StockEntry[] stocks;
	public Human humanPrefab;
	public Bot botPrefab;

	public static GameManager Instance { get; private set; }
	public Dictionary<StockType, Stock> Stocks { get; private set; }
	public List<Player> Players { get; private set; }
	public List<Bot> Bots { get; private set; }
	public Dictionary<int, Player> PlayersByActorID { get; private set; }
	public Player Player { get; private set; }
	public Queue<int> SyncQueue { get; private set; }

	private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;
		Stocks = new Dictionary<StockType, Stock>();
		Players = new List<Player>();
		Bots = new List<Bot>();
		PlayersByActorID = new Dictionary<int, Player>();
		foreach (StockEntry stockEntry in stocks) {
			Stocks.Add(stockEntry.stockType, stockEntry.stock);
		}
		resolverQueue = new ResolverQueue();
		SyncQueue = new Queue<int>();
	}

	void Start() {
		// Create player at location determined by player pos
		Player[] playersByPos = new Player[7];
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
			int pos = (int)PhotonNetwork.PlayerList[i].CustomProperties[PlayerProperty.Pos];
			float playerAngle = pos * 360f / 7;
			Vector3 playerPosition = new Vector3(
				Mathf.Sin(Mathf.Deg2Rad * playerAngle) * 40,
				0,
				-Mathf.Cos(Mathf.Deg2Rad * playerAngle) * 40
			);
			Quaternion playerRotation = Quaternion.Euler(0, -playerAngle, 0);
			
			Player player = Instantiate(humanPrefab, playerPosition, playerRotation);
			PlayersByActorID[PhotonNetwork.PlayerList[i].ActorNumber] = player;
			if (PhotonNetwork.PlayerList[i].IsLocal) {
				Player = player;
			}

			playersByPos[pos] = player;
		}

		// Create bots to fill in remaining pos
		for (int pos = PhotonNetwork.PlayerList.Length; pos < 7; pos++) {
			float playerAngle = pos * 360f / 7;
			Vector3 playerPosition = new Vector3(
				Mathf.Sin(Mathf.Deg2Rad * playerAngle) * 40,
				0,
				-Mathf.Cos(Mathf.Deg2Rad * playerAngle) * 40
			);
			Quaternion playerRotation = Quaternion.Euler(0, -playerAngle, 0);
			
			Bot bot = Instantiate(botPrefab, playerPosition, playerRotation);
			Bots.Add(bot);

			playersByPos[pos] = bot;
		}

		// Store players in a compact list ordered by pos
		for (int i = 0; i < playersByPos.Length; i++) {
			if (playersByPos[i] != null) {
				Players.Add(playersByPos[i]);
			}
		}

		// Assign neighbours
		for (int i = 0; i < Players.Count; i++) {
			Players[i].Neighbours[Direction.West] = Players[(Players.Count + i - 1) % Players.Count];
			Players[i].Neighbours[Direction.East] = Players[(i + 1) % Players.Count];
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

	public void DecideBuild(int positionInHand) {
		photonView.RPC("DecideBuild", RpcTarget.All, positionInHand);
	}

	public void DecideBury(int positionInHand, int wonderStage) {
		photonView.RPC("DecideBury", RpcTarget.All, positionInHand, wonderStage);
	}

	public void DecideDiscard(int positionInHand) {
		photonView.RPC("DecideDiscard", RpcTarget.All, positionInHand);
	}

	public void DecideBotBuild(Bot bot, int positionInHand) {
		int botIndex = 0;
		for (int i = 0; i < Bots.Count; i++) {
			if (bot == Bots[i]) {
				botIndex = i;
				break;
			}
		}
		photonView.RPC("DecideBotBuild", RpcTarget.All, botIndex, positionInHand);
	}

	public void DecideBotBury(Bot bot, int positionInHand, int wonderStage) {
		int botIndex = 0;
		for (int i = 0; i < Bots.Count; i++) {
			if (bot == Bots[i]) {
				botIndex = i;
				break;
			}
		}
		photonView.RPC("DecideBotBury", RpcTarget.All, botIndex, positionInHand, wonderStage);
	}

	public void DecideBotDiscard(Bot bot, int positionInHand) {
		int botIndex = 0;
		for (int i = 0; i < Bots.Count; i++) {
			if (bot == Bots[i]) {
				botIndex = i;
				break;
			}
		}
		photonView.RPC("DecideBotDiscard", RpcTarget.All, botIndex, positionInHand);
	}

	public void Sync() {
		photonView.RPC("Sync", RpcTarget.All);
	}

	[PunRPC]
	private void DecideBuild(int positionInHand, PhotonMessageInfo info) {
		Player player = PlayersByActorID[info.Sender.ActorNumber];
		StartCoroutine(player.PrepareBuild(positionInHand));
	}

	[PunRPC]
	private void DecideBury(int positionInHand, int wonderStage, PhotonMessageInfo info) {
		Player player = PlayersByActorID[info.Sender.ActorNumber];
		StartCoroutine(player.PrepareBury(positionInHand, wonderStage));
	}

	[PunRPC]
	private void DecideDiscard(int positionInHand, PhotonMessageInfo info) {
		Player player = PlayersByActorID[info.Sender.ActorNumber];
		StartCoroutine(player.PrepareDiscard(positionInHand));
	}

	[PunRPC]
	private void DecideBotBuild(int botIndex, int positionInHand) {
		Player player = Bots[botIndex];
		StartCoroutine(player.PrepareBuild(positionInHand));
	}

	[PunRPC]
	private void DecideBotBury(int botIndex, int positionInHand, int wonderStage) {
		Player player = Bots[botIndex];
		StartCoroutine(player.PrepareBury(positionInHand, wonderStage));
	}

	[PunRPC]
	private void DecideBotDiscard(int botIndex, int positionInHand) {
		Player player = Bots[botIndex];
		StartCoroutine(player.PrepareDiscard(positionInHand));
	}

	[PunRPC]
	private void Sync(PhotonMessageInfo info) {
		SyncQueue.Enqueue(info.Sender.ActorNumber);
	}

	private IEnumerator Resolve() {
		while (resolverQueue.Size() != 0) {
			yield return resolverQueue.Dequeue().Resolve();
			Debug.Log("Queue size: " + resolverQueue.Size());
		}
	}

}