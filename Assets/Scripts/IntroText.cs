using UnityEngine;
using System.Collections;

public class IntroText : MonoBehaviour {

	private float timer = 0.0f;

	public Font font;
	GUIStyle style = new GUIStyle();

	public int level;


	// Use this for initialization
	void Start () {
		style.font = font;
		style.fontSize = 30;
		style.normal.textColor = Color.white;
		style.alignment = TextAnchor.MiddleCenter;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}

	void OnGUI() {
		Rect position = new Rect(0, 0, Screen.width, Screen.height);
		
		if(timer < 1.0f) {

		}
		else if(timer < 6.0f) {
			GUI.Label(position, "I helped them take over the city.", style);
		}
		else if(timer < 11.0f) {
			GUI.Label(position, "Then, I was a loose end.", style);
		}
		else if(timer > 14.0f) {
			Application.LoadLevel(level);
		}
	}
}
