using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	public int state = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float newAlpha = guiTexture.color.a;
		if(state == 1) {
			newAlpha = Mathf.Lerp(guiTexture.color.a, 1.0f, Time.deltaTime * 5.0f);
		}
		if(state == 0) {
			newAlpha = Mathf.Lerp(guiTexture.color.a, 0.0f, Time.deltaTime * 5.0f);
		}

		guiTexture.color = new Color(guiTexture.color.r, guiTexture.color.g, guiTexture.color.b, newAlpha);
	}

	public void FadeIn() {
		state = 1;

	}

	public void FadeOut() {
		state = 0;
	}
}
