using UnityEngine;

public class RoomPlayer : MonoBehaviour {

	public TextMesh nicknameMesh;
	public MeshRenderer readyMesh;

	private bool ready;

	public string Nickname {
		get {
			return nicknameMesh.text;
		}

		set {
			nicknameMesh.text = value;
		}
	}

	public bool IsReady {
		get {
			return ready;
		}

		set {
			ready = value;
			readyMesh.gameObject.SetActive(value);
		}
	}

}
