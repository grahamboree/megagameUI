using System.Collections;
using DG.Tweening;
using Kino;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {
	[SerializeField] TMP_InputField InputField;
	[SerializeField] GameObject AccessDenied;
	[SerializeField] Camera Camera;
	[SerializeField] CanvasGroup LoginItems;
	[SerializeField] Image Overlay;

	const string startingText = "enter access code...";
	const float successfulLoginFadeTime = 5;

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
		var accessCode = InputField.text.Trim().ToLowerInvariant();
		Debug.Log("Got Access Code: " + accessCode);

		InputField.text = "";
		yield return null;

		var inputSystem = FindObjectOfType<EventSystem>();
		inputSystem.gameObject.SetActive(false);

		yield return new WaitForSeconds(0.2f);

		inputSystem.gameObject.SetActive(true);

		bool passwordValid = DataManager.Instance.Data.ContainsKey(accessCode);
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
			CurrentSession.Instance.ClearCurrentSession();
			CurrentSession.Instance.AccessKey = accessCode;

			LoginItems.DOFade(0, 0.5f);

			Camera.transform.DOMove(new Vector3(0, 0, -3f), successfulLoginFadeTime);
			FindObjectOfType<GlitchController>().enabled = false;
//			FindObjectOfType<DigitalGlitch>().enabled = false;
			FindObjectOfType<AnalogGlitch>().enabled = false;

			var glitch = FindObjectOfType<DigitalGlitch>();
			DOTween.To(
				() => glitch.intensity,
				x => glitch.intensity = x,
				1,
				3)
				.SetDelay(successfulLoginFadeTime - 3);

			Overlay.DOFade(1, successfulLoginFadeTime / 2f)
				.SetDelay(successfulLoginFadeTime / 2f)
				.SetEase(Ease.InExpo);

			yield return new WaitForSeconds(successfulLoginFadeTime);

			SceneManager.LoadScene("Booting", LoadSceneMode.Single);
		}

		yield return null;

		ResetInputField();
		loggingIn = false;
	}
}
