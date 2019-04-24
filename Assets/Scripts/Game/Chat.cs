using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour {

	public Vector2 collapsedPosition;
	public Vector2 expandedPosition;
	public InputField chatInput;
	public Text[] chatTexts;
	public Image unreadPanel;
	
	private RectTransform rectTransform;
	private bool expanded;

	void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}

	public void Toggle(bool expanded) {
		rectTransform.anchoredPosition = expanded ? expandedPosition : collapsedPosition;
		this.expanded = expanded;

		if (expanded) {
			SetRead();
		}
	}

	public void Send() {
		string message = chatInput.text.Trim();
		chatInput.text = "";

		if (message == "") {
			// Ignore empty chat message
			return;
		}

		NetworkManager.Instance.SendChat(
			string.Format("<b>{0}</b>: {1}", GameManager.Instance.Player.Nickname, message)
		);
	}

	public void AddMessage(string message) {
		for (int i = chatTexts.Length - 1; i > 0; i--) {
			chatTexts[i].text = chatTexts[i - 1].text;
		}
		chatTexts[0].text = message;

		if (!expanded) {
			SetUnread();
		}
	}

	private void SetUnread() {
		unreadPanel.gameObject.SetActive(true);
	}

	private void SetRead() {
		unreadPanel.gameObject.SetActive(false);
	}

}
