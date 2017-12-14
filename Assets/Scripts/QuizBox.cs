using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuizBox : MonoBehaviour {

	QuizParser quizParser;
	ScreenTextParser screenTextParser;
	public string quizQuestions;

	private string question;
	private string ans1;
	private string ans2;
	private string ans3;
	private string ans4;
	private int numAns;
	private List<string> charsForAns1;
	private List<string> charsForAns2;
	private List<string> charsForAns3;
	private List<string> charsForAns4;


	//count which character player chooses based on answers
	List<string> characterCount;
	private int quizFileLineCount;

	// taking the quiz
	private bool takingQuiz;

	// keep line number count
	int quizLineNum;
	int screenTextLineNum;
	private int screenTextFileLineCount;
	string currScreenText;
	Color color;
	bool textStillDisplaying;
	public float fadeFloat;
	private bool textDisplaySection1;
	private bool textDisplaySection2;


	public GUIStyle questionStyle, answerStyle, displayText;

	//player stats
	public static Player playa;
	public InputField nameField;
	private GameObject gameCanvas;
	private string playerCharName;
	private bool gettingName;

	// Use this for initialization
	void Start () {
		// player info
		playa = GameObject.Find("Player").GetComponent<Player>();

		// start display text
		currScreenText = "";
		screenTextParser = GameObject.Find("ScreenTextParserObject").GetComponent<ScreenTextParser>();
		screenTextParser.fileName = playa.getScreenTextToDisplay ();
		screenTextParser.reloadScreenText ();
		screenTextFileLineCount = screenTextParser.GetLineCount ();
		screenTextLineNum = 0;
		textStillDisplaying = false;
		takingQuiz = false;
		color = Color.white;
		fadeFloat = 1.5f;
		textDisplaySection1 = true;
		textDisplaySection2 = false;

		// name input
		gettingName = false;
		gameCanvas = GameObject.Find ("Canvas");
		gameCanvas.SetActive (gettingName);



		// quiz section
		quizQuestions = "";
		quizParser = GameObject.Find("QuizParserObject").GetComponent<QuizParser>();
		quizParser.fileName = playa.getQuizQuestions();
		quizParser.loadQuestions();
		characterCount =  new List<string> ();
		quizFileLineCount = quizParser.GetLineCount ();

		quizLineNum = 0;

		setBackground();
	}
	
	// Update is called once per frame
	void Update () {

		setBackground ();

		// displaying start text
		if (!takingQuiz && !gettingName) {
			
			if ((screenTextLineNum == screenTextFileLineCount) && textDisplaySection1) {
				gettingName = true;
				textDisplaySection1 = false;
				playa.setScreenTextToDisplay ("WelcomePlayerName.txt");
				screenTextParser.fileName = playa.getScreenTextToDisplay ();
				screenTextParser.reloadScreenText ();
				screenTextFileLineCount = screenTextParser.GetLineCount ();
				screenTextLineNum = 0;
				textStillDisplaying = false;
				StopAllCoroutines ();

			} else if ((screenTextLineNum == screenTextFileLineCount) && textDisplaySection2) {
				textDisplaySection2 = false;	
				takingQuiz = true;
				StopAllCoroutines ();
			} else {
				displayScreenText ();
			}
				
		}

		// getting player name
		if (gettingName) {

			// set the button and text field size based on the screen size
			Button button = gameCanvas.GetComponentInChildren<Button> ();
			InputField input = gameCanvas.GetComponentInChildren<InputField> ();

			Text inputText1 = input.transform.Find ("Text").gameObject.GetComponent<Text>();
			Text inputText2 = input.transform.Find ("Placeholder").gameObject.GetComponent<Text>();
			inputText1.fontSize = Mathf.RoundToInt (((Screen.width * .2f) + (Screen.height * .32f)) / 7f);
			inputText2.fontSize = Mathf.RoundToInt (((Screen.width * .2f) + (Screen.height * .32f)) / 7f);

			input.characterLimit = 20;

			float worldScreenHeight = Camera.main.orthographicSize * 2;
			float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

			/*
			Text [] inputTexts = input.GetComponentsInChildren<Text> ();

			for (int i = 0; i < inputTexts.Count(); i++) {
				inputTexts[i].fontSize = Mathf.RoundToInt(((Screen.width * .2f) + (Screen.height * .32f)) / 7f);
			} 
			*/

			input.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Screen.width * .6f);
			input.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Screen.height * .2f);

			button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 (0, -worldScreenHeight * 9f, 0);
			button.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Screen.width * .3f);
			button.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Screen.height * .1f);

			// display the canvas
			gameCanvas.SetActive(gettingName);

			// once done, take the quiz
		}

		// when done with quiz
		if (quizLineNum == quizFileLineCount) {
			//calculate the number of names to figure out what shape the player is
			playa.setPlayerShape (calculateShape ());
		}

	}

	public void onSubmit() {
		// check to make sure that the player has inputted correct info
		if (performInputCheck ()) {
			playerCharName = nameField.text;

			Debug.Log ("You selected name: " + playerCharName);
			playa.setPlayerName (playerCharName);

			Debug.Log ("Player name is also " + playa.getPlayerName ());
			gettingName = false;
			textDisplaySection2 = true;
			gameCanvas.SetActive (gettingName);
		}
	}

	private bool performInputCheck() {
		bool toReturn = false;

		if (nameField.text == "") {
			// get the popup game object that's attached to the input field
			InputField input = gameCanvas.GetComponentInChildren<InputField> ();
			GameObject popUp = input.transform.Find ("PopUp").gameObject;
			Text popUpText = popUp.GetComponentInChildren<Text> ();

			// set the font and size
			popUpText.fontSize = Mathf.RoundToInt(((Screen.width * .2f) + (Screen.height * .32f)) / 20f);

			popUp.GetComponent <RectTransform> ().anchoredPosition3D = new Vector3 (0, input.GetComponent<RectTransform>().sizeDelta.y * 2.5f, 0);
			popUp.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Screen.width * .3f);
			popUp.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Screen.height * .2f);

			popUp.SetActive (true);

			return toReturn;
		} else {
			toReturn = true;
		}

		return toReturn;
	}

	void displayScreenText() {
		if (!textStillDisplaying) {
			string tempString = screenTextParser.getScreenTextAt (screenTextLineNum);
			tempString = replaceWithPlayerName (tempString);
			currScreenText = tempString;
			textStillDisplaying = true;
			StartCoroutine (FadeInText (fadeFloat));
		}
	}


	private string replaceWithPlayerName(string toCheckAndReplace){
		return toCheckAndReplace.Replace ("[PLAYERNAME]", playa.getPlayerName ());;
	}


	IEnumerator FadeInText(float t)
	{
		Debug.Log ("in fade in text");
		color = new Color (color.r, color.g, color.b, 0);
		while (color.a < 1.0f) {
			color.a = color.a + (Time.deltaTime / t);
			Debug.Log ("waiting for fade in");
			yield return new WaitForSeconds(Time.deltaTime / t);

		}

		Debug.Log ("waiting for text display");
		yield return new WaitForSeconds (fadeFloat);

		Debug.Log ("done waiting for text display");
		yield return StartCoroutine (FadeOutText (t));
	}


	IEnumerator FadeOutText(float t)
	{
		Debug.Log ("in fade out text");
		color = new Color (color.r, color.g, color.b, 1);
		while (color.a > 0.0f) {
			color.a = color.a - (Time.deltaTime / t);
			Debug.Log ("waiting for fade out");
			yield return new WaitForSeconds(Time.deltaTime / t);
		}
		textStillDisplaying = false;
		screenTextLineNum = screenTextLineNum + 1;
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

	private string calculateShape() {
		int squareCount = 0;
		int triangleCount = 0;
		int rectangleCount = 0;
		int circleCount = 0;
		int maxCount = 0;
		string toReturn = "";

		for (int i = 0; i < characterCount.Count; i++) {
			
			switch (characterCount.ElementAt(i))
			{
			case "Square":
				squareCount = squareCount + 1;
				if (maxCount < squareCount) {
					maxCount = squareCount;
					toReturn = "Square";
				}
				break;
			case "Rectangle":
				rectangleCount = rectangleCount + 1;
				if (maxCount < rectangleCount) {
					maxCount = rectangleCount;
					toReturn = "Rectangle";
				}
				break;
			case "Triangle":
				triangleCount = triangleCount + 1;
				if (maxCount < triangleCount) {
					maxCount = triangleCount;
					toReturn = "Triangle";
				}
				break;
			case "Circle":
				circleCount = circleCount + 1;
				if (maxCount < circleCount) {
					maxCount = circleCount;
					toReturn = "Circle";
				}
				break;
			default:
				Debug.Log ("ERROR: Issue in QuizBox.cs - calculateShape() - Shape is not identifiable: " + characterCount.ElementAt (i));
				break;   
			}

		}

		return toReturn;
	}


	void OnGUI()
	{
		if (textDisplaySection1 || textDisplaySection2) {
			GUI.FocusControl(null);
			displayText.normal.textColor = color;
			displayText.wordWrap = true;
			GUI.color = color;
			displayText.fontSize = (Screen.width + Screen.height) / 40;
			GUI.TextField(new Rect(Screen.width * (.15f), Screen.height * (.3f), Screen.width * (.7f), Screen.height * (.3f)), currScreenText, displayText);
		}
		else if (takingQuiz) {
			// taking the quiz
			questionStyle.fontSize = Mathf.RoundToInt (((Screen.width * .2f) + (Screen.height * .32f)) / 10f);
			answerStyle.fontSize = Mathf.RoundToInt (((Screen.width * .2f) + (Screen.height * .32f)) / 15f);
			questionStyle.wordWrap = true;
			answerStyle.wordWrap = true;

			question = quizParser.getQuestion (quizLineNum);
			ans1 = quizParser.getAnswer1 (quizLineNum);
			ans2 = quizParser.getAnswer2 (quizLineNum);
			ans3 = quizParser.getAnswer3 (quizLineNum);
			ans4 = quizParser.getAnswer4 (quizLineNum);
			numAns = quizParser.getNumAnswers (quizLineNum);

			GUI.Label (new Rect (Screen.width * (.1f), Screen.height * (0), Screen.width * (.8f), Screen.height * (.3f)), question, questionStyle);
			// pending on number of answers, display appropriately
			if (numAns == 2) {
				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.32f), Screen.width * (.6f), Screen.height * (.30f)), ans1, answerStyle)) {
					// option 0
					charsForAns1 = quizParser.getCharactersForAns1 (quizLineNum);
					characterCount.AddRange (charsForAns1);
					quizLineNum = quizLineNum + 1;
				}

				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.30f)), ans2, answerStyle)) {
					// option 1
					charsForAns2 = quizParser.getCharactersForAns2 (quizLineNum);
					characterCount.AddRange (charsForAns2);
					quizLineNum = quizLineNum + 1;
				}
			} else if (numAns == 3) {
				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.30f), Screen.width * (.6f), Screen.height * (.15f)), ans1, answerStyle)) {
					// option 0
					charsForAns1 = quizParser.getCharactersForAns1 (quizLineNum);
					characterCount.AddRange (charsForAns1);
					quizLineNum = quizLineNum + 1;
				}

				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.48f), Screen.width * (.6f), Screen.height * (.15f)), ans2, answerStyle)) {
					// option 1
					charsForAns2 = quizParser.getCharactersForAns2 (quizLineNum);
					characterCount.AddRange (charsForAns2);
					quizLineNum = quizLineNum + 1;
				}

				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.15f)), ans3, answerStyle)) {
					// option 2
					charsForAns3 = quizParser.getCharactersForAns3 (quizLineNum);
					characterCount.AddRange (charsForAns3);
					quizLineNum = quizLineNum + 1;
				}
		
			} else if (numAns == 4) {
			
				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.30f), Screen.width * (.6f), Screen.height * (.15f)), ans1, answerStyle)) {
					// option 0
					charsForAns1 = quizParser.getCharactersForAns1 (quizLineNum);
					characterCount.AddRange (charsForAns1);
					quizLineNum = quizLineNum + 1;				
				}

				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.48f), Screen.width * (.6f), Screen.height * (.15f)), ans2, answerStyle)) {
					// option 1
					charsForAns2 = quizParser.getCharactersForAns2 (quizLineNum);
					characterCount.AddRange (charsForAns2);
					quizLineNum = quizLineNum + 1;
				}

				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.15f)), ans3, answerStyle)) {
					// option 2
					charsForAns3 = quizParser.getCharactersForAns3 (quizLineNum);
					characterCount.AddRange (charsForAns3);
					quizLineNum = quizLineNum + 1;
				}
				if (GUI.Button (new Rect (Screen.width * (.2f), Screen.height * (.82f), Screen.width * (.6f), Screen.height * (.15f)), ans4, answerStyle)) {
					// option 3
					charsForAns4 = quizParser.getCharactersForAns4 (quizLineNum);
					characterCount.AddRange (charsForAns4);
					quizLineNum = quizLineNum + 1;
				}
			}
		}
	}



}
