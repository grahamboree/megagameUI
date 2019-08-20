using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LogoutScreen : MonoBehaviour {
    [SerializeField] TextMeshProUGUI ReminderText;
    [SerializeField] TextMeshProUGUI AutoRestartText;

    //////////////////////////////////////////////////

    static readonly string[] messages = {
        "Check for tails, twice on PARs.",
        "(WOODS TIP) Remember, there is no lake.",
        "Remember to tag in all G.S.D. equipment, including your corporeal and incorporeal forms, at the end of shift.",
        "Decontamination is vital- evil has done more with less.",
        "The integrity of reality rests on your #2 Ticonderoga Pencil",
        "Elder Gods never miss a deadline, neither should you!",
        "The public are ants- be a mirror, not a magnifying glass.",
        "Protocols are like dominatrices- to be obeyed while wearing the proper gear.",
        "Observe your protocols and your coworkers.",
        "(WOODS TIP) Look ahead, look around, never look behind.",
        "(WOODS TIP) The trees can hear, but do not speak."
    };

    void Start() {
        StartCoroutine(RestartSequence());
    }

    IEnumerator RestartSequence() {
        var eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.enabled = false;

        ReminderText.enabled = false;
        AutoRestartText.enabled = false;

        yield return new WaitForSeconds(0.25f);

        ReminderText.enabled = true;
        AutoRestartText.enabled = true;

        string tip = messages[Random.Range(0, messages.Length)];
        ReminderText.text = "<size=+10><color=white>Logged out</size></color>\n\n\n\n\n\n\n<size=+30>" + tip;

        float restartTime = Time.time + 10;
        while (Time.time < restartTime) {
            float timeLeft = restartTime - Time.time;
            float fpart = timeLeft - Mathf.Floor(timeLeft);

            string autoRestartText = "System shutdown commencing in " + Mathf.Ceil(timeLeft);

            if (fpart < 0.25f) {
                autoRestartText += "...";
            } else if (fpart < 0.5f) {
                autoRestartText += "..";
            } else if (fpart < 0.75f) {
                autoRestartText += ".";
            }

            AutoRestartText.text = autoRestartText;
            yield return null;
        }

        eventSystem.enabled = true;
        SceneManager.LoadScene("Login");
    }
}
