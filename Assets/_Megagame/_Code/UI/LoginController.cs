using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Megagame._Code.UI {
	public class LoginController : MonoBehaviour {
		[SerializeField] TMP_InputField InputField;

		const string startingText = "enter access code...";

		void Start() {
			InputField.onSubmit.AddListener(OnInputSubmitted);
			ResetInputField();
		}

		void Update() {
			bool removeStartingText = Input.anyKeyDown &&
			                          !Input.GetMouseButtonDown(0) &&
			                          !Input.GetMouseButtonDown(1) &&
			                          InputField.text.StartsWith(startingText);

			if (removeStartingText) {
				var text = InputField.text.Remove(0, startingText.Length);
				InputField.contentType = TMP_InputField.ContentType.Password;
				InputField.text = text; // after we switch, so it updates the first letter
			}

			if (!InputField.isFocused) {
				FocusInputField();
			}
		}

		void OnDestroy() {
			InputField.onSubmit.RemoveListener(OnInputSubmitted);
		}

		void ResetInputField() {
			InputField.text = startingText;
			InputField.contentType = TMP_InputField.ContentType.Standard;

			FocusInputField();
		}

		void OnInputSubmitted(string text) {
			StartCoroutine(AttemptingLogin());
		}

		void FocusInputField() {
			StartCoroutine(FocusingInputField());
		}

		IEnumerator FocusingInputField() {
			InputField.Select();
			InputField.ActivateInputField();
			yield return null;
			InputField.MoveTextEnd(false);
		}

		IEnumerator AttemptingLogin() {
			var accessCode = InputField.text;
			var anim = GetComponent<Animator>();
			Debug.Log("Got Access Code: " + accessCode);

			anim.SetTrigger("next"); // to "Checking password"

			InputField.text = "";

			var inputSystem = FindObjectOfType<EventSystem>();
			inputSystem.gameObject.SetActive(false);

			yield return new WaitForSeconds(0.5f);

			inputSystem.gameObject.SetActive(true);

			bool passwordValid = InputField.text == "banana";

			anim.SetTrigger("next"); // to "Access Denied"
			anim.SetBool("password_valid", passwordValid);
		}
	}
}
