using UnityEngine;
using System.Collections;

public class HintScript : MonoBehaviour {

	private GameObject player;

	public float radius;
	public string text;

	public Font font;

	GUIStyle style = new GUIStyle();

	private float visible = 0;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");

		style.font = font;
		style.fontSize = 30;
		style.normal.textColor = Color.white;
		style.alignment = TextAnchor.MiddleCenter;
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, player.transform.position) < radius) {
			visible = 5.0f;
		}
		visible -= Time.deltaTime;
	}

	void OnGUI() {
		Rect position = new Rect(0, 0, Screen.width, Screen.height);

		if(visible > 0) {
			GUI.Label(position, text, style);
		}
	}
}
