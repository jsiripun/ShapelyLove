using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture;  		// texture to be used for fade
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;   		// order of draw hierarchy, low number renders on top
	private float alpha = 1.0f;				// texture's alpha value between 0 and 1
	private int fadeDir = -1;				// the direction to fade: in = -1; out = 1

	void OnGUI () {
		// fade out/in the alpha value using direction, speed, and time.deltatime to convert operation to seconds
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force (clamp) number between 0 and 1 because GUI.color uses alpha between 0 and 1
		alpha = Mathf.Clamp01(alpha);

		// set color of GUI (texture). all color values remain same and alpha is set to alpha variable
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);				// set alpha value
		GUI.depth = drawDepth;																// make texture render on top
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);		// draw texture to fit screen area
	}

	public float BeginFade (int direction) {
		fadeDir = direction;
		return (fadeSpeed);
	}

	void OnLevelWasLoaded () {
		BeginFade (-1);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}