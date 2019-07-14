using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour {
	public static DataManager Instance;

	const string spreadsheetURL = "https://docs.google.com/spreadsheets/d/1g88q39eU_w5V6V03h5SSqBDth2AQO9hIFWpS0KM7tDI/export?format=tsv";

	public Dictionary<string, List<DataEntry>> Data = new Dictionary<string, List<DataEntry>>();

	float nextUpdateTime = 0f;
	float updateInterval = 999990;

	void Awake() {
		Instance = this;
	}

	void Update() {
		if (Time.unscaledTime >= nextUpdateTime) {
			nextUpdateTime = Time.unscaledTime + updateInterval;
			StartCoroutine(DownloadGoogleSheetTSV());
		}
	}

	IEnumerator DownloadGoogleSheetTSV() {
		var www = UnityWebRequest.Get(spreadsheetURL);
		yield return www.SendWebRequest();
		var spreadsheet = www.downloadHandler.text;
		UpdateWithSpreadsheet(spreadsheet);
	}

	void UpdateWithSpreadsheet(string spreadsheet) {
		var lines = spreadsheet.Split('\n');

		Data.Clear();

		// Parse header and extract keys:
		var users = new List<string>();
		var headerCells = lines[0].Split('\t');
		for (int colIndex = 2; colIndex < headerCells.Length; ++colIndex) {
			var user = headerCells[colIndex].Trim().ToLowerInvariant();
			users.Add(user);
			Data[user] = new List<DataEntry>();
		}

		Debug.Log("Got users: " + string.Join(", ", users.ToArray()));

		// Skip first line, because it's header.
		foreach (var line in lines.Skip(1)) {
			var cells = line.Split('\t');

			var dataEntry = new DataEntry(cells);

			// Add data entry to access list for all users.
			for (int colIndex = 2; colIndex < cells.Length; ++colIndex) {
				if (cells[colIndex].Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) {
					var user = users[colIndex - 2];
					Data[user].Add(dataEntry);
				}
			}
		}
	}
}

public class DataEntry {
	public string Name;
	public string Description;

	public DataEntry(string[] cells) {
		Name = cells[0];
		Description = cells[1];
	}
}
