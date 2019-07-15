using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NReco.Csv;
using UnityEngine;
using UnityEngine.Networking;

public class DataEntry {
	public string Name;
	public string Description;
}

public class DataManager : MonoBehaviour {
	public static DataManager Instance;
	public Dictionary<string, List<DataEntry>> Data = new Dictionary<string, List<DataEntry>>();

	//////////////////////////////////////////////////

	const string spreadsheetCSV = "https://docs.google.com/spreadsheets/d/1g88q39eU_w5V6V03h5SSqBDth2AQO9hIFWpS0KM7tDI/export?format=csv";

	float nextUpdateTime;
	float updateInterval = 999990;

	//////////////////////////////////////////////////

	void Awake() {
		Instance = this;
	}

	void Update() {
		if (Time.unscaledTime >= nextUpdateTime) {
			nextUpdateTime = Time.unscaledTime + updateInterval;
			StartCoroutine(DownloadGoogleSheet(spreadsheetCSV));
		}
	}

	IEnumerator DownloadGoogleSheet(string url) {
		var www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
		var spreadsheet = www.downloadHandler.text;
		UpdateWithCSV(spreadsheet);
	}

	void UpdateWithCSV(string csv) {
		Data.Clear();

		using (var streamRdr = new StringReader(csv)) {
			var csvReader = new CsvReader(streamRdr, ",");

			bool isHeader = true;
			var users = new List<string>();

			while (csvReader.Read()) {
				if (isHeader) { // Parse header and extract keys:
					isHeader = false;

					for (int colIndex = 2; colIndex < csvReader.FieldsCount; ++colIndex) {
						var user = csvReader[colIndex].Trim().ToLowerInvariant();
						users.Add(user);
						Data[user] = new List<DataEntry>();
					}

					Debug.Log("Got users: " + string.Join(", ", users.ToArray()));
				} else {
					var dataEntry = new DataEntry {
						Name = csvReader[0],
						Description = DressUpDescription(csvReader[1])
					};

					// Add data entry to access list for all users.
					for (int colIndex = 2; colIndex < csvReader.FieldsCount; ++colIndex) {
						if (csvReader[colIndex].Trim().Equals("true", StringComparison.OrdinalIgnoreCase)) {
							var user = users[colIndex - 2];
							Data[user].Add(dataEntry);
						}
					}
				}
			}
		}
	}

	string DressUpDescription(string description) {
		description = Regex.Replace(
			description,
			@"Designation:",
			"<uppercase><color #F3F3DB><size=+5>Designation:</size></color></uppercase>",
			RegexOptions.IgnoreCase);
		description = Regex.Replace(
			description,
			@"Subject:",
			"<uppercase><color #F3F3DB><size=+5>Subject:</size></color></uppercase>",
			RegexOptions.IgnoreCase);
		description = Regex.Replace(
			description,
			@"Procedure:",
			"<uppercase><color #F3F3DB><size=+5>Procedure:</size></color></uppercase>",
			RegexOptions.IgnoreCase);
		description = Regex.Replace(
			description,
			@"Properties:",
			"<uppercase><color #F3F3DB><size=+5>Properties:</size></color></uppercase>",
			RegexOptions.IgnoreCase);
		description = Regex.Replace(
			description,
			@"Findings:",
			"<uppercase><color #F3F3DB><size=+5>Findings:</size></color></uppercase>",
			RegexOptions.IgnoreCase);
		description = Regex.Replace(
			description,
			@"Ongoing:",
			"<uppercase><color #F3F3DB><size=+5>Ongoing:</size></color></uppercase>",
			RegexOptions.IgnoreCase);
		return description;
	}
}
