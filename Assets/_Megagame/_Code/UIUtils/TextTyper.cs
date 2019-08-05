using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextTyper : MonoBehaviour {
	public float totalRevealTime = 10;

	public void SetString(string value) {
		GetComponent<TextMeshProUGUI>().text = "";
		goalText = value;
		var chars = goalText.ToCharArray();
		charsToShow = new Queue<char>(chars);
		secondsBetweenChars = totalRevealTime / chars.Length;
		nextCharTime = Time.unscaledTime;
	}

	string goalText = "";
	Queue<char> charsToShow = new Queue<char>();
	float secondsBetweenChars = 0.001f;
	float nextCharTime = 0;

	void Start() {
		var startingText = GetComponent<TextMeshProUGUI>().text;
		SetString(startingText);
	}

	void Update() {
		var tmp = GetComponent<TextMeshProUGUI>();
		while (charsToShow.Count > 0 && Time.unscaledTime >= nextCharTime) {
			nextCharTime += secondsBetweenChars;

			// Add a char
			var charToAdd = charsToShow.Dequeue();
			tmp.text += charToAdd;

			// skip over rich text tags
			if (charToAdd == '<') {
				while (charToAdd != '>') {
					charToAdd = charsToShow.Dequeue();
					tmp.text += charToAdd;
				}
			}
		}
	}
}
