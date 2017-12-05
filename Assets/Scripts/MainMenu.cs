using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void LoadStartScene ()
	{
		SceneManager.LoadScene("OnTrain");
	}

	public void LoadOptionsScene()
	{
		SceneManager.LoadScene("Options");
	}

}