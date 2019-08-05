using System.Collections;
using UnityEngine;

public class TextCycler : MonoBehaviour {
    [SerializeField] TextTyper typer;

    void Start() {
        StartCoroutine(ChangeText());
    }

    IEnumerator ChangeText() {
        string[] text = {
            "Surface Temp: 17.2c\nVisibility: 17km\nPressure: 1532.3mb\nWind: N/NW 10-15km/h\nForecast: ???",
            "System Status: nominal\nFacility Power: 582 MW/h\nSecurity Feed: online\nBoron Levels: 23ppm\nThreat Level: Epsilon",
            "Mainframe Load: 92%\nCompute Units: 209k\nDatabase Capacity: 72%\nData Throughput: 3.4 TB/h",
            "<size=+23><font=\"Gobotronic SDF\">HELPMEIMTRAPPEDHEL",
        };

        int currentLine = 0;

        while (true) {
            yield return null;
            typer.SetString(text[currentLine]);
            yield return new WaitForSeconds(8);
            currentLine = (currentLine + 1) % text.Length;
        }
    }
}
