using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class SyncResolver : IResolvable {

	private Queue<int> waitSyncQueue;

	public SyncResolver() {
		waitSyncQueue = new Queue<int>();
		foreach (Photon.Realtime.PhotonPlayer photonPlayer in PhotonNetwork.PlayerList) {
			waitSyncQueue.Enqueue(photonPlayer.ActorNumber);
		}
	}

	public IEnumerator Resolve() {
		GameManager.Instance.Sync();
		
		while (waitSyncQueue.Count != 0) {
			if (GameManager.Instance.SyncQueue.Count == 0) {
				yield return null;
			} else if (waitSyncQueue.Peek() == GameManager.Instance.SyncQueue.Peek()) {
				waitSyncQueue.Dequeue();
				GameManager.Instance.SyncQueue.Dequeue();
			} else {
				waitSyncQueue.Enqueue(waitSyncQueue.Dequeue());
				yield return null;
			}
		}
	}

}
