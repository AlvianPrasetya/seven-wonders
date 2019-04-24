using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager Instance { get; private set; }
	public PlayerNav[] playerNavs;
	public Text timerText;
	public Scoreboard scoreboard;
	public Chat chat;

	private Coroutine countdown;

	void Awake() {
		Instance = this;
	}

	void Start() {
		float leftSafeMargin = 1920 * Screen.safeArea.xMin / Screen.width;
		chat.collapsedPosition.x += leftSafeMargin;
		chat.expandedPosition.x += leftSafeMargin;
		chat.Toggle(false);
	}

	public void StartTimer(float time) {
		timerText.gameObject.SetActive(true);

		countdown = StartCoroutine(Countdown(time));
	}

	public void StopTimer() {
		if (countdown != null) {
			StopCoroutine(countdown);
			countdown = null;
		}

		timerText.gameObject.SetActive(false);
	}

	private IEnumerator Countdown(float time) {
		while (time > 0) {
			timerText.text = time.ToString("0");
			time -= Time.deltaTime;
			yield return null;
		}

		countdown = null;
	}

}
