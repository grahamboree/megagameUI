using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataTest : MonoBehaviour {
	[Header("Index")]
	[SerializeField] ScrollRect IndexSR;
	[SerializeField] Button IndexButtonPrefab;

	[Header("Description")]
	[SerializeField] ScrollRect scrollRect;
	[SerializeField] TextMeshProUGUI title;
	[SerializeField] TextMeshProUGUI description;
	//[SerializeField] TextTyper description;

	void Start() {
		var data = DataManager.Instance.Data[CurrentSession.Instance.AccessKey];

		// Create index buttons
		for (int datumIndex = 0; datumIndex < data.Count; datumIndex++) {
			var datum = data[datumIndex];
			var button = Instantiate(IndexButtonPrefab, IndexButtonPrefab.transform.parent);
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<TextMeshProUGUI>().text = datum.Name;
			button.GetComponent<Button>().onClick.AddListener(() => { ShowData(datumIndex); });
		}

		IndexSR.verticalNormalizedPosition = 0;

		ShowData(0);
	}

	public void ShowData(int index) {
		var items = DataManager.Instance.Data[CurrentSession.Instance.AccessKey];
		title.text = items[index].Name;
		description.text = items[index].Description;
		scrollRect.verticalNormalizedPosition = 0;
	}
}
