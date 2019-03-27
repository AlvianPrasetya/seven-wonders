using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager Instance { get; private set; }
	public Text timerText;

	private Coroutine countdown;

	void Awake() {
		Instance = this;
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
