using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DataTest : MonoBehaviour {
	[SerializeField] LogoutScreen logoutScreen;

		[Header("Index")]
	[SerializeField] GameObject IndexEntryPrefab;

	[Header("Description")]
	[SerializeField] ScrollRect scrollRect;
	[SerializeField] TextMeshProUGUI title;
	[SerializeField] TextMeshProUGUI description;

	[Header("Components")]
	[SerializeField] GameObject Container;
	[SerializeField] GameObject ListingColumn;
	[SerializeField] GameObject LogoutButton;
	[SerializeField] GameObject FileName;
	[SerializeField] GameObject FileContents;
	[SerializeField] GameObject RandomText;
	[SerializeField] GameObject Sphere;
	[SerializeField] GameObject[] Graphs;

	//////////////////////////////////////////////////

	public void LogOut() {
		logoutScreen.gameObject.SetActive(true);
		logoutScreen.transform.DOScaleY(0, 0.25f).From();
	}

	//////////////////////////////////////////////////

	Toggle firstToggle;

	//////////////////////////////////////////////////

	void Start() {
		if (DataManager.Instance != null) {
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
					if (isOn && gameObject.activeSelf) {
						ShowData(index);
					}
				});
				if (datumIndex == 0) {
					firstToggle = toggle;
				}
			}
		}

		StartCoroutine(StartupSequence());
	}

	float delayBetween = 0.5f;

	IEnumerator StartupSequence() {
		var items = DataManager.Instance.Data[CurrentSession.Instance.AccessKey];

		Container.SetActive(false);
		ListingColumn.SetActive(false);
		LogoutButton.SetActive(false);
		FileName.SetActive(false);
		FileContents.SetActive(false);
		RandomText.SetActive(false);
		Sphere.SetActive(false);
		foreach (var graph in Graphs) {
			graph.SetActive(false);
		}

		yield return null;

		Container.SetActive(true);

		Container.transform.DOScaleY(0, 1f).From();

		yield return new WaitForSeconds(1f);

		ListingColumn.SetActive(true);
		ListingColumn.transform.DOScaleX(0, delayBetween).From();
		yield return null;
		firstToggle.isOn = true;

		yield return new WaitForSeconds(0.1f);
		LogoutButton.SetActive(true);
		LogoutButton.transform.DOScaleX(0, delayBetween).From();

		yield return new WaitForSeconds(0.1f);
		FileName.SetActive(true);
		FileName.transform.DOScaleY(0, delayBetween).From();
		yield return null;
		title.text = items[0].Name;

		yield return new WaitForSeconds(0.1f);
		FileContents.SetActive(true);
		description.text = "";
		FileContents.transform.DOScaleY(0, delayBetween * 2).From();
		yield return null;
		description.text = items[0].Description;

		yield return null;
		if (DataManager.Instance != null) {
			firstToggle.isOn = true;
			ShowData(0);
		}
		yield return new WaitForSeconds(0.1f);
		RandomText.SetActive(true);
		RandomText.transform.DOScaleX(0, delayBetween).From()
			.OnComplete(() => {
				Sphere.SetActive(true);
			});
		foreach (var graph in Graphs) {
			yield return new WaitForSeconds(0.1f);
			graph.SetActive(true);
			graph.transform.DOScaleX(0, delayBetween).From();
		}

		yield return new WaitForSeconds(delayBetween);
	}

	void ShowData(int index) {
		StartCoroutine(ShowingData(index));
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
