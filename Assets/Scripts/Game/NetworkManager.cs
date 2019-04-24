using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviour, IOnEventCallback {

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

	void Awake() {
		Instance = this;
	}

	void OnEnable() {
		PhotonNetwork.AddCallbackTarget(this);
	}

	void OnDisable() {
		PhotonNetwork.RemoveCallbackTarget(this);
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
		PhotonNetwork.RaiseEvent(
			CodeDecideDraft,
			new object[] { positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideBuild(int positionInHand, Payment payment) {
		PhotonNetwork.RaiseEvent(
			CodeDecideBuild,
			new object[] { positionInHand, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideBury(int positionInHand, int wonderStage, Payment payment) {
		PhotonNetwork.RaiseEvent(
			CodeDecideBury,
			new object[] { positionInHand, wonderStage, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideDiscard(int positionInHand) {
		PhotonNetwork.RaiseEvent(
			CodeDecideDiscard,
			new object[] { positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideCycle(Direction direction) {
		PhotonNetwork.RaiseEvent(
			CodeDecideCycle,
			new object[] { direction },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideBotDraft(Bot bot, int positionInHand) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}
		
		PhotonNetwork.RaiseEvent(
			CodeDecideBotDraft,
			new object[] { botIndex, positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideBotBuild(Bot bot, int positionInHand, Payment payment) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}
		
		PhotonNetwork.RaiseEvent(
			CodeDecideBotBuild,
			new object[] { botIndex, positionInHand, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideBotBury(Bot bot, int positionInHand, int wonderStage, Payment payment) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}
		
		PhotonNetwork.RaiseEvent(
			CodeDecideBotBury,
			new object[] { botIndex, positionInHand, wonderStage, payment },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void DecideBotDiscard(Bot bot, int positionInHand) {
		int botIndex = 0;
		for (int i = 0; i < GameManager.Instance.Bots.Count; i++) {
			if (bot == GameManager.Instance.Bots[i]) {
				botIndex = i;
				break;
			}
		}
		
		PhotonNetwork.RaiseEvent(
			CodeDecideBotDiscard,
			new object[] { botIndex, positionInHand },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

	public void Sync() {
		PhotonNetwork.RaiseEvent(
			CodeSync,
			new object[] {},
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}
	
	public void SendChat(string message) {
		PhotonNetwork.RaiseEvent(
			CodeSendChat,
			new object[] { message },
			new RaiseEventOptions{ Receivers = ReceiverGroup.All },
			new ExitGames.Client.Photon.SendOptions { Reliability = true }
		);
	}

}
