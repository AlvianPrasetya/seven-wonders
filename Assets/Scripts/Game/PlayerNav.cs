using UnityEngine;
using UnityEngine.UI;

public class PlayerNav : MonoBehaviour {

	public Button playerNavButton;
	public Text playerNavText;

	private Player player;

	public Player Player {

		set {
			player = value;
			playerNavText.text = value.Nickname;
		}

	}

	public void Navigate() {
		StartCoroutine(GameManager.Instance.gameCamera.Focus(player));
	}

}
