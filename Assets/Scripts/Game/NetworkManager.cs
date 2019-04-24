using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback {

	private struct PendingEvent {

		public byte EventCode { get; private set; }
		public object EventContent { get; private set; }
		public RaiseEventOptions RaiseEventOptions { get; private set; }
		public ExitGames.Client.Photon.SendOptions SendOptions { get; private set; }

		public PendingEvent(
			byte eventCode, object eventContent,
			RaiseEventOptions raiseEventOptions, ExitGames.Client.Photon.SendOptions sendOptions
		) {
			EventCode = eventCode;
			EventContent = eventContent;
			RaiseEventOptions = raiseEventOptions;
			SendOptions = sendOptions;
		}

	}

	private const byte CodeDecideDraft = 0;
	private const byte CodeDecideBuild = 1;
	private const byte CodeDecideBury = 2;
	private const byte CodeDecideDiscard = 3;
	private const byte CodeDecideCycle = 4;
	private const byte CodeDecideBotDraft = 5;
	private const byte CodeDecideBotBuild = 6;
	private const byte CodeDecideBotBury = 7;
	private const byte CodeDecideBotDiscard = 8;
	private const byte CodeSync = 9;
	private const byte CodeSendChat = 10;

	public static NetworkManager Instance { get; private set; }

	private Queue<PendingEvent> pendingEvents;

	void Awake() {
		Instance = this;
		pendingEvents = new Queue<PendingEvent>();
	}

	void Start() {
		StartCoroutine(RaiseEvents());
	}

	public override void OnEnable() {
		PhotonNetwork.AddCallbackTarget(this);
	}

	public override void OnDisable() {
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public override void OnDisconnected(DisconnectCause cause) {
		Debug.LogFormat("Disconnected due to {0}", cause);

		PhotonNetwork.ReconnectAndRejoin();
	}
	
	void IOnEventCallback.OnEvent(ExitGames.Client.Photon.EventData photonEvent) {
		object[] data = (object[])photonEvent.CustomData;
		switch (photonEvent.Code) {
			case CodeDecideDraft:
				GameManager.Instance.HandleDecideDraft((int)data[0], photonEvent.Sender);
				break;
			case CodeDecideBuild:
				GameManager.Instance.HandleDecideBuild((int)data[0], (Payment)data[1], photonEvent.Sender);
				break;
			case CodeDecideBury:
				GameManager.Instance.HandleDecideBury(
					(int)data[0], (int)data[1], (Payment)data[2], photonEvent.Sender
				);
				break;
			case CodeDecideDiscard:
				GameManager.Instance.HandleDecideDiscard((int)data[0], photonEvent.Sender);
				break;
			case CodeDecideCycle:
				GameManager.Instance.HandleDecideCycle((Direction)data[0], photonEvent.Sender);
				break;
			case CodeDecideBotDraft:
				GameManager.Instance.HandleDecideBotDraft((int)data[0], (int)data[1]);
				break;
			case CodeDecideBotBuild:
				GameManager.Instance.HandleDecideBotBuild((int)data[0], (int)data[1], (Payment)data[2]);
				break;
			case CodeDecideBotBury:
				GameManager.Instance.HandleDecideBotBury(
					(int)data[0], (int)data[1], (int)data[2], (Payment)data[3]
				);
				break;
			case CodeDecideBotDiscard:
				GameManager.Instance.HandleDecideBotDiscard((int)data[0], (int)data[1]);
				break;
			case CodeSync:
				GameManager.Instance.HandleSync(photonEvent.Sender);
				break;
			case CodeSendChat:
				GameManager.Instance.HandleSendChat((string)data[0]);
				break;
		}
	}

	public void DecideDraft(int positionInHand) {
		pendingEvents.Enqueue(new PendingEvent(
			CodeDecideDraft,
			new object[] { positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));
	}

	public void DecideBuild(int positionInHand, Payment payment) {
		pendingEvents.Enqueue(new PendingEvent(
			CodeDecideBuild,
			new object[] { positionInHand, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));
	}

	public void DecideBury(int positionInHand, int wonderStage, Payment payment) {
		pendingEvents.Enqueue(new PendingEvent(
			CodeDecideBury,
			new object[] { positionInHand, wonderStage, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));
	}

	public void DecideDiscard(int positionInHand) {
		pendingEvents.Enqueue(new PendingEvent(
			CodeDecideDiscard,
			new object[] { positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));
	}

	public void DecideCycle(Direction direction) {
		pendingEvents.Enqueue(new PendingEvent(
			CodeDecideCycle,
			new object[] { direction },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));
	}

	public void DecideBotDraft(Bot bot, int positionInHand) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}

		GameManager.Instance.HandleDecideBotDraft(botIndex, positionInHand);
		
		/*pendingEvents.Enqueue(new PendingEvent(
			CodeDecideBotDraft,
			new object[] { botIndex, positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));*/
	}

	public void DecideBotBuild(Bot bot, int positionInHand, Payment payment) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}
		
		GameManager.Instance.HandleDecideBotBuild(botIndex, positionInHand, payment);

		/*pendingEvents.Enqueue(new PendingEvent(
			CodeDecideBotBuild,
			new object[] { botIndex, positionInHand, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));*/
	}

	public void DecideBotBury(Bot bot, int positionInHand, int wonderStage, Payment payment) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}

		GameManager.Instance.HandleDecideBotBury(botIndex, positionInHand, wonderStage, payment);
		
		/*pendingEvents.Enqueue(new PendingEvent(
			CodeDecideBotBury,
			new object[] { botIndex, positionInHand, wonderStage, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));*/
	}

	public void DecideBotDiscard(Bot bot, int positionInHand) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}
		
		GameManager.Instance.HandleDecideBotDiscard(botIndex, positionInHand);

		/*pendingEvents.Enqueue(new PendingEvent(
			CodeDecideBotDiscard,
			new object[] { botIndex, positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));*/
	}

	public void Sync() {
		pendingEvents.Enqueue(new PendingEvent(
			CodeSync,
			new object[] {},
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));
	}
	
	public void SendChat(string message) {
		pendingEvents.Enqueue(new PendingEvent(
			CodeSendChat,
			new object[] { message },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		));
	}

	private IEnumerator RaiseEvents() {
		while (true) {
			if (pendingEvents.Count != 0) {
				PendingEvent pendingEvent = pendingEvents.Peek();
				if (PhotonNetwork.RaiseEvent(
					pendingEvent.EventCode,
					pendingEvent.EventContent,
					pendingEvent.RaiseEventOptions,
					pendingEvent.SendOptions
				)) {
					pendingEvents.Dequeue();
					continue;
				}
			}

			yield return null;
		}
	}

}
