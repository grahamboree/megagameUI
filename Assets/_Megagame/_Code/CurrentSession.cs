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
		if (Input.GetKey(KeyCode.Escape)) {
			if (string.IsNullOrEmpty(AccessKey)) {
				Application.Quit();
			} else {
				ClearCurrentSession();
				SceneManager.LoadScene("Login", LoadSceneMode.Single);
			}
		}
	}
}
