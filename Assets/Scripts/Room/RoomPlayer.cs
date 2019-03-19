using UnityEngine;
using UnityEngine.UI;

public class RoomPlayer : MonoBehaviour {

	public Text nicknameText;
	public Image readyImage;

	private bool ready;

	public string Nickname {
		get {
			return nicknameText.text;
		}

		set {
			nicknameText.text = value;
		}
	}

	public bool IsReady {
		get {
			return ready;
		}

		set {
			ready = value;
			readyImage.gameObject.SetActive(value);
		}
	}

}
