using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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


	public GUIStyle questionStyle, answerStyle, displayText;

	//player stats
	public static Player playa;


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

		if (!takingQuiz) {
			
				displayScreenText ();

				if (screenTextLineNum == screenTextFileLineCount) {
					takingQuiz = true;
				}
		}



		if (quizLineNum == quizFileLineCount) {
			//calculate the number of names to figure out what shape the player is
			playa.setPlayerShape (calculateShape ());
		}

	}


	void displayScreenText() {
		if (!textStillDisplaying) {
			currScreenText = screenTextParser.getScreenTextAt (screenTextLineNum);
			textStillDisplaying = true;
			StartCoroutine (FadeInText (fadeFloat));
		}
	}





	IEnumerator FadeInText(float t)
	{
		color = new Color (color.r, color.g, color.b, 0);
		while (color.a < 1.0f) {
			color.a = color.a + (Time.deltaTime / t);
			yield return new WaitForSeconds(Time.deltaTime / t);

		}

		yield return new WaitForSeconds (1.5f);

		yield return StartCoroutine (FadeOutText (t));
	}


	IEnumerator FadeOutText(float t)
	{
		color = new Color (color.r, color.g, color.b, 1);
		while (color.a > 0.0f) {
			color.a = color.a - (Time.deltaTime / t);
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
		if (!takingQuiz) {
			displayText.normal.textColor = color;
			displayText.wordWrap = true;
			GUI.color = color;
			displayText.fontSize = (Screen.width + Screen.height) / 40;
			GUI.TextField(new Rect(Screen.width * (.15f), Screen.height * (.3f), Screen.width * (.7f), Screen.height * (.3f)), currScreenText, displayText);
		}
		else {
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
