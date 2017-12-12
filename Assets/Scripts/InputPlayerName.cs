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
	}

	public void onSubmit() {
		charName = nameField.text;

		Debug.Log ("You selected name: " + charName);
		playa.setPlayerName (charName);

		Debug.Log ("Player name is also " + playa.getPlayerName ());
	}
}
