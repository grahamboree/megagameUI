using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LineTyper : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float revealTime;

    string goalText;
    string[] lines;

    float startTime;

    void Start() {
        goalText = text.text;
        lines = goalText.Split('\n');

        text.text = "";

        startTime = Time.time;
    }

    void Update() {
        float sinceStart = Time.time - startTime;
        int linesToReveal = Mathf.CeilToInt((sinceStart / revealTime) * lines.Length);
        text.text = string.Join("\n", lines.Take(linesToReveal).Reverse().Take(90).Reverse().ToArray());
        if (sinceStart > revealTime) {
            SceneManager.LoadScene("Data", LoadSceneMode.Single);
        }
    }
}
