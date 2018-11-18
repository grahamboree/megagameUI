using System.Collections;
using TMPro;
using UnityEngine;

namespace _Megagame._Code.UIUtils {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextFlasher : MonoBehaviour {
		[SerializeField] float timePerFlash = 0.1f;
		[SerializeField] float timeBetweenFlash= 0.05f;

		TextMeshProUGUI Text;
		bool visible = false;
		float timeOfNextTransition = float.MinValue;

		void Start() {
			Text = GetComponent<TextMeshProUGUI>();
		}

		void OnEnable() {
			timeOfNextTransition = float.MinValue;
			visible = false;
			Debug.Log("resetting flasher");
		}

		void Update() {
			if (timeOfNextTransition < Time.time) {
				visible = !visible;
				timeOfNextTransition = Time.time + (visible ? timePerFlash : timeBetweenFlash);

				var newColor = Text.color;
				newColor.a = visible ? 1.0f : 0.0f;
				Text.color = newColor;
			}
		}
	}
}
