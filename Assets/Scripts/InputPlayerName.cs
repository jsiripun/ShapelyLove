using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputPlayerName : MonoBehaviour {

	public InputField nameField;
	public static Player playa;

	private string charName;

	// Use this for initialization
	void Start () {
		playa = GameObject.Find ("Player").GetComponent<Player>();

		setBackground ();
	}

	public void onSubmit() {
		charName = nameField.text;

		Debug.Log ("You selected name: " + charName);
		playa.setPlayerName (charName);

		Debug.Log ("Player name is also " + playa.getPlayerName ());
	}

	void setBackground()
	{
		GameObject background = GameObject.Find("background");

		SpriteRenderer sr = background.GetComponent<SpriteRenderer>();

		float worldScreenHeight = Camera.main.orthographicSize * 2;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		background.transform.localScale = new Vector3(
			worldScreenWidth / sr.sprite.bounds.size.x,
			worldScreenHeight / sr.sprite.bounds.size.y, 1);
	}
}
