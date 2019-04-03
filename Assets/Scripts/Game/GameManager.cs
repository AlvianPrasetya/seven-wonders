using Photon.Pun;
using System;
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
	public Dictionary<int, Player> HumansByActorID { get; private set; }
	public Player Player { get; private set; }
	public List<Human> Humans { get; private set; }
	public List<Bot> Bots { get; private set; }
	public Queue<int> SyncQueue { get; private set; }

	private ResolverQueue resolverQueue;

	void Awake() {
		Instance = this;
		Stocks = new Dictionary<StockType, Stock>();
		Players = new List<Player>();
		HumansByActorID = new Dictionary<int, Player>();
		foreach (StockEntry stockEntry in stocks) {
			Stocks.Add(stockEntry.stockType, stockEntry.stock);
		}
		Humans = new List<Human>();
		Bots = new List<Bot>();
		SyncQueue = new Queue<int>();
		resolverQueue = new ResolverQueue();
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
			
			Human human = Instantiate(humanPrefab, playerPosition, playerRotation);
			human.Nickname = PhotonNetwork.PlayerList[i].NickName;
			HumansByActorID[PhotonNetwork.PlayerList[i].ActorNumber] = human;
			if (PhotonNetwork.PlayerList[i].IsLocal) {
				Player = human;
			}
			Humans.Add(human);

			playersByPos[pos] = human;
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
			bot.Nickname = string.Format("Bot {0}", pos + 1 - PhotonNetwork.PlayerList.Length);
			Bots.Add(bot);

			playersByPos[pos] = bot;
		}

		// Store players in a compact list ordered by pos
		for (int i = 0; i < playersByPos.Length; i++) {
			if (playersByPos[i] != null) {
				Players.Add(playersByPos[i]);
				// Set hand facing down for other players
				playersByPos[i].hand.Facing = (playersByPos[i] == Player) ? Facing.Up : Facing.Down;
			}
		}

		// Assign neighbours
		for (int i = 0; i < Players.Count; i++) {
			Player westPlayer = Players[(Players.Count + i - 1) % Players.Count];
			Player eastPlayer = Players[(i + 1) % Players.Count];
			Players[i].Neighbours[Direction.West] = westPlayer;
			Players[i].Neighbours[Direction.East] = eastPlayer;

			// Set initial buying costs
			foreach (Resource resource in Enum.GetValues(typeof(Resource))) {
				Players[i].ResourceBuyCosts[new Player.PlayerResource(westPlayer, resource)] = GameOptions.InitialBuyCost;
				Players[i].ResourceBuyCosts[new Player.PlayerResource(eastPlayer, resource)] = GameOptions.InitialBuyCost;
			}
		}

		int playerPos = (int)PhotonNetwork.LocalPlayer.CustomProperties[PlayerProperty.Pos];
		for (int i = -Players.Count / 2; i < Players.Count / 2f; i++) {
			Player player = playersByPos[(playerPos + i + Players.Count) % Players.Count];
			UIManager.Instance.playerNavs[i + Players.Count / 2].Player = player;
		}

		resolverQueue.Enqueue(
			new MatchResolver((int)PhotonNetwork.CurrentRoom.CustomProperties[MatchSeedKey]),
			Priority.ResolveMatch
		);
		StartCoroutine(Resolve());
	}

	public void EnqueueResolver(IResolvable resolver, int priority) {
		resolverQueue.Enqueue(resolver, priority);
	}

	public void EnqueueResolver(IResolvable[] resolvers, int priority) {
		foreach (IResolvable resolver in resolvers) {
			resolverQueue.Enqueue(resolver, priority);
		}
	}

	public void DecideBuild(int positionInHand, Payment payment) {
		Debug.LogFormat(
			"Paying bank: {0}, west: {1}, east: {2}",
			payment.PayBankAmount, payment.PayWestAmount, payment.PayEastAmount
		);
		photonView.RPC(
			"DecideBuild", RpcTarget.All, positionInHand,
			payment.PayBankAmount, payment.PayWestAmount, payment.PayEastAmount
		);
	}

	public void DecideBury(int positionInHand, int wonderStage, Payment payment) {
		Debug.LogFormat(
			"Paying bank: {0}, west: {1}, east: {2}",
			payment.PayBankAmount, payment.PayWestAmount, payment.PayEastAmount
		);
		photonView.RPC(
			"DecideBury", RpcTarget.All, positionInHand, wonderStage,
			payment.PayBankAmount, payment.PayWestAmount, payment.PayEastAmount
		);
	}

	public void DecideDiscard(int positionInHand) {
		photonView.RPC("DecideDiscard", RpcTarget.All, positionInHand);
	}

	public void DecideBotBuild(Bot bot, int positionInHand, Payment payment) {
		int botIndex = 0;
		for (int i = 0; i < Bots.Count; i++) {
			if (bot == Bots[i]) {
				botIndex = i;
				break;
			}
		}
		photonView.RPC(
			"DecideBotBuild", RpcTarget.All, botIndex, positionInHand,
			payment.PayBankAmount, payment.PayWestAmount, payment.PayEastAmount
		);
	}

	public void DecideBotBury(Bot bot, int positionInHand, int wonderStage, Payment payment) {
		int botIndex = 0;
		for (int i = 0; i < Bots.Count; i++) {
			if (bot == Bots[i]) {
				botIndex = i;
				break;
			}
		}
		photonView.RPC(
			"DecideBotBury", RpcTarget.All, botIndex, positionInHand, wonderStage,
			payment.PayBankAmount, payment.PayWestAmount, payment.PayEastAmount
		);
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
	private void DecideBuild(
		int positionInHand,
		int payBankAmount, int payWestAmount, int payEastAmount,
		PhotonMessageInfo info
	) {
		Player player = HumansByActorID[info.Sender.ActorNumber];
		Payment payment = new Payment(payBankAmount, payWestAmount, payEastAmount);
		StartCoroutine(player.PrepareBuild(positionInHand, payment));
	}

	[PunRPC]
	private void DecideBury(
		int positionInHand, int wonderStage,
		int payBankAmount, int payWestAmount, int payEastAmount,
		PhotonMessageInfo info
	) {
		Player player = HumansByActorID[info.Sender.ActorNumber];
		Payment payment = new Payment(payBankAmount, payWestAmount, payEastAmount);
		StartCoroutine(player.PrepareBury(positionInHand, wonderStage, payment));
	}

	[PunRPC]
	private void DecideDiscard(int positionInHand, PhotonMessageInfo info) {
		Player player = HumansByActorID[info.Sender.ActorNumber];
		StartCoroutine(player.PrepareDiscard(positionInHand));
	}

	[PunRPC]
	private void DecideBotBuild(
		int botIndex, int positionInHand,
		int payBankAmount, int payWestAmount, int payEastAmount
	) {
		Player player = Bots[botIndex];
		Payment payment = new Payment(payBankAmount, payWestAmount, payEastAmount);
		StartCoroutine(player.PrepareBuild(positionInHand, payment));
	}

	[PunRPC]
	private void DecideBotBury(
		int botIndex, int positionInHand, int wonderStage,
		int payBankAmount, int payWestAmount, int payEastAmount
	) {
		Player player = Bots[botIndex];
		Payment payment = new Payment(payBankAmount, payWestAmount, payEastAmount);
		StartCoroutine(player.PrepareBury(positionInHand, wonderStage, payment));
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