using System;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour {

	public string logFilename = "7wonders.log";

	private StreamWriter writer;
	
	void OnEnable() {
		string logFilePath = Application.persistentDataPath + "/" + logFilename;

		writer = new StreamWriter(logFilePath, true);
		writer.AutoFlush = true;

		Application.logMessageReceived += OnLoggedCallback;
	}

	void OnDisable() {
		Application.logMessageReceived -= OnLoggedCallback;

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
