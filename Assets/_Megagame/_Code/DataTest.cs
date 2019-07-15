using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataTest : MonoBehaviour {
	[Header("Index")]
	[SerializeField] GameObject IndexEntryPrefab;

	[Header("Description")]
	[SerializeField] ScrollRect scrollRect;
	[SerializeField] TextMeshProUGUI title;
	[SerializeField] TextMeshProUGUI description;

	void Start() {
//		GetComponent<Canvas>().worldCamera = Camera.main;
		var data = DataManager.Instance.Data[CurrentSession.Instance.AccessKey];

		// Create index buttons
		for (int datumIndex = 0; datumIndex < data.Count; datumIndex++) {
			var datum = data[datumIndex];
			var button = Instantiate(IndexEntryPrefab, IndexEntryPrefab.transform.parent);
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<TextMeshProUGUI>().text = datum.Name;
			int index = datumIndex;
			//button.GetComponent<Button>().onClick.AddListener(() => { ShowData(index); });
			var toggle = button.GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(isOn => {
				if (isOn && gameObject.active) {
					ShowData(index);
				}
			});
			if (datumIndex == 0) {
				firstToggle = toggle;
			}
		}

		firstToggle.isOn = true;

		ShowData(0);
	}

	Toggle firstToggle;

	public void ShowData(int index) {
		StartCoroutine(ShowingData(index));
	}

	public void LogOut() {
		SceneManager.LoadScene("Login");
	}

	IEnumerator ShowingData(int index) {
		var items = DataManager.Instance.Data[CurrentSession.Instance.AccessKey];
		title.text = items[index].Name;
		description.text = "\n" + items[index].Description + "\n";
		yield return null;
		yield return null;
		scrollRect.verticalNormalizedPosition = 1;
	}
}
