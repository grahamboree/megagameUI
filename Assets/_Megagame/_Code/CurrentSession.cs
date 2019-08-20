using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSession : MonoBehaviour {
	public static CurrentSession Instance;

	public string AccessKey;

	void Awake() {
		Instance = this;
	}

	public void ClearCurrentSession() {
		AccessKey = null;
	}

	void Update() {
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.X)) {
			if (string.IsNullOrEmpty(AccessKey)) {
				Application.Quit();
			} else {
				ClearCurrentSession();
				SceneManager.LoadScene("Login", LoadSceneMode.Single);
			}
		}
	}
}
