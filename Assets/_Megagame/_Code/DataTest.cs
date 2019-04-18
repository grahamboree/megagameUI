using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataTest : MonoBehaviour {
	[SerializeField] ScrollRect scrollRect;
	[SerializeField] TextMeshProUGUI title;
	[SerializeField] TextTyper description;
	[SerializeField] Image image;

	[Space(20)]
	[SerializeField] ImageMap[] images;

	int currentIndex = 0;

	void Start() {
		ShowData();
	}

	void ShowData() {
		var items = DataManager.Instance.Data[CurrentSession.Instance.AccessKey];
		title.text = items[currentIndex].Name;
		description.SetString(items[currentIndex].Description);
		image.sprite = GetImage(items[currentIndex].Name);
	}

	void Update() {
		int delta = 0;

		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			delta = -1;
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			delta = 1;
		}

		if (delta != 0) {
			var items = DataManager.Instance.Data[CurrentSession.Instance.AccessKey];
			currentIndex += delta;
			if (currentIndex < 0) {
				currentIndex = items.Count - 1;
			} else if (currentIndex >= items.Count) {
				currentIndex = 0;
			}
			ShowData();
		}
		scrollRect.verticalNormalizedPosition = 0;
	}

	Sprite GetImage(string name) {
		foreach (var img in images) {
			if (img.name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
				return img.sprite;
			}
		}

		return null;
	}
}

[Serializable]
public class ImageMap {
	public string name;
	public Sprite sprite;
}
