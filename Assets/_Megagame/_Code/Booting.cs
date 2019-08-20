using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Booting : MonoBehaviour {
    [SerializeField] TextMeshProUGUI Text;
    IEnumerator Start() {
        string text = "<startup sequence initiated>";

        int characters = 0;

        for (int i = 0; i < text.Length + 1; ++i) {
            Text.text = text.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Startup Sequence", LoadSceneMode.Single);
    }
}
