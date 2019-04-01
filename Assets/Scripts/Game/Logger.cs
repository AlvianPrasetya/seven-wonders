using System;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour {

	public string logFilename = "7wonders.log";

	private StreamWriter writer;
	
	void Awake() {
		string logFilePath = Application.persistentDataPath + "/" + logFilename;

		if (!File.Exists(logFilePath)) {
			try {
				File.Create(logFilePath);
			} catch (System.Exception e) {
				Debug.LogErrorFormat("Failed to create log file: {0}", e);
				return;
			}
		}

		writer = new StreamWriter(logFilePath, true);

		Application.logMessageReceived += OnLoggedCallback;
	}

	void OnDestroy() {
		try {
			writer.Close();
		} catch (System.Exception e) {
			Debug.LogErrorFormat("Failed to close log writer: {0}", e);
		}
	}

	private void OnLoggedCallback(string condition, string stackTrace, LogType type) {
		try {
			writer.WriteLine(
				"[{0}] {1}: {2} {3}",
				DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
				type,
				condition,
				stackTrace
			);
		} catch (System.Exception e) {
			Debug.LogErrorFormat("Failed to write log: {0}", e);
		}
	}

}
