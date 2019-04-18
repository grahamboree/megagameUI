using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginController : MonoBehaviour {
	[SerializeField] TMP_InputField InputField;

	[SerializeField] GameObject AccessDenied;
	[SerializeField] 

	const string startingText = "enter access code...";

	bool loggingIn = false;

	void Start() {
		InputField.onSubmit.AddListener(OnInputSubmitted);
		ResetInputField();
	}

	void Update() {
		if (!loggingIn) {
			if (Input.anyKeyDown && InputField.text.StartsWith(startingText)) {
				var text = InputField.text.Remove(0, startingText.Length);
				InputField.contentType = TMP_InputField.ContentType.Password;
				InputField.text = text; // after we switch, so it updates the first letter
			}

			if (!InputField.isFocused) {
				StartCoroutine(FocusInputField());
			}
		}
	}

	void OnDestroy() {
		InputField.onSubmit.RemoveListener(OnInputSubmitted);
	}

	void ResetInputField() {
		InputField.text = startingText;
		InputField.contentType = TMP_InputField.ContentType.Standard;

		StartCoroutine(FocusInputField());
	}

	void OnInputSubmitted(string text) {
		StartCoroutine(AttemptingLogin());
	}

	IEnumerator FocusInputField() {
		InputField.Select();
		InputField.ActivateInputField();
		yield return null;
		InputField.MoveTextEnd(false);
	}

	IEnumerator AttemptingLogin() {
		loggingIn = true;
		var accessCode = InputField.text;
		Debug.Log("Got Access Code: " + accessCode);

		InputField.text = "";
		yield return null;

		var inputSystem = FindObjectOfType<EventSystem>();
		inputSystem.gameObject.SetActive(false);

		yield return new WaitForSeconds(0.2f);

		inputSystem.gameObject.SetActive(true);

		bool passwordValid = accessCode == "banana";
		if (!passwordValid) {
			AccessDenied.gameObject.SetActive(true);
			AccessDenied.transform.DOScaleY(0, 0.2f).From();

			var accessDeniedText = AccessDenied.GetComponentInChildren<TextMeshProUGUI>();

			for (int i = 0; i < 3; ++i) {
				accessDeniedText.enabled = false;
				yield return new WaitForSeconds(0.2f);
				accessDeniedText.enabled = true;
				yield return new WaitForSeconds(0.2f);
			}
			accessDeniedText.enabled = false;

			AccessDenied.transform.DOScaleY(0, 0.2f);
			yield return new WaitForSeconds(0.2f);

			// Reset and disable
			AccessDenied.transform.localScale = Vector3.one;
			accessDeniedText.enabled = true;
			AccessDenied.gameObject.SetActive(false);
		} else {

		}

		yield return null;

		ResetInputField();
		loggingIn = false;
	}
}
