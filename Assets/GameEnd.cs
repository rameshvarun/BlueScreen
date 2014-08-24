using UnityEngine;
using System.Collections;

public class GameEnd : MonoBehaviour {

	private float timer = 0.0f;
	
	public Font font;
	GUIStyle style = new GUIStyle();


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
		
		if(timer < 4.0f) {
			
		}
		/*else if(timer < 9.0f) {
			if(!audio.isPlaying)
				audio.Play();

			GUI.Label(position, "Guess I'll live to see another das", style);
		}*/
		else {
			if(!audio.isPlaying)
				audio.Play();

			GUI.Label(position, "Thanks for playing", style);
		}
	}
}
