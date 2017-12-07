using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class QuizBox : MonoBehaviour {

	QuizParser parser;
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
	private List<string> characterCount;
	private int fileLineCount;

	int lineNum;

	public GUIStyle questionStyle, answerStyle;

	//player stats
	public static Player playa;


	// Use this for initialization
	void Start () {
		quizQuestions = "";
		parser = GameObject.Find("QuizParserObject").GetComponent<QuizParser>();
		playa = GameObject.Find("Player").GetComponent<Player>();
		parser.fileName = playa.getQuizQuestions();
		parser.loadQuestions();
		fileLineCount = parser.GetLineCount ();
		characterCount = new List<string> ();
		lineNum = 0;
		setBackground();
	}
	
	// Update is called once per frame
	void Update () {

		setBackground ();

		if (lineNum == fileLineCount) {
			//calculate the number of names to figure out what shape the player is
			playa.setPlayerShape (calculateShape ());
		}

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

		Debug.Log ("square count: " + squareCount);
		Debug.Log ("rectangle count: " + rectangleCount);
		Debug.Log ("triangle count: " + triangleCount);
		Debug.Log ("circle count: " + circleCount);
		Debug.Log ("you are a " + toReturn);

		return toReturn;
	}


	void OnGUI()
	{
		questionStyle.fontSize = Mathf.RoundToInt(((Screen.width * .2f) + (Screen.height * .32f)) / 10f);
		answerStyle.fontSize = Mathf.RoundToInt(((Screen.width * .2f) + (Screen.height * .32f)) / 15f);
		questionStyle.wordWrap = true;
		answerStyle.wordWrap = true;

		question = parser.getQuestion(lineNum);
		ans1 = parser.getAnswer1(lineNum);
		ans2 = parser.getAnswer2(lineNum);
		ans3 = parser.getAnswer3(lineNum);
		ans4 = parser.getAnswer4(lineNum);
		numAns = parser.getNumAnswers(lineNum);

		GUI.Label(new Rect(Screen.width * (.1f), Screen.height * (0), Screen.width * (.8f), Screen.height * (.3f)), question, questionStyle);
		// pending on number of answers, display appropriately
		if (numAns == 2) {
			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.32f), Screen.width * (.6f), Screen.height * (.30f)), ans1, answerStyle))
			{
				// option 0
				charsForAns1 = parser.getCharactersForAns1(lineNum);
				characterCount.AddRange (charsForAns1);
				lineNum = lineNum+1;
			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.30f)), ans2, answerStyle))
			{
				// option 1
				charsForAns2 = parser.getCharactersForAns2(lineNum);
				characterCount.AddRange (charsForAns2);
				lineNum = lineNum+1;
			}
		}
		else if (numAns == 3) {
			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.30f), Screen.width * (.6f), Screen.height * (.15f)), ans1, answerStyle))
			{
				// option 0
				charsForAns1 = parser.getCharactersForAns1(lineNum);
				characterCount.AddRange (charsForAns1);
				lineNum = lineNum+1;
			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.48f), Screen.width * (.6f), Screen.height * (.15f)), ans2, answerStyle))
			{
				// option 1
				charsForAns2 = parser.getCharactersForAns2(lineNum);
				characterCount.AddRange (charsForAns2);
				lineNum = lineNum+1;
			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.15f)), ans3, answerStyle))
			{
				// option 2
				charsForAns3 = parser.getCharactersForAns3(lineNum);
				characterCount.AddRange (charsForAns3);
				lineNum = lineNum+1;
			}
		
		} 
		else if (numAns == 4) {
			
		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.30f), Screen.width * (.6f), Screen.height * (.15f)), ans1, answerStyle))
			{
				// option 0
				charsForAns1 = parser.getCharactersForAns1(lineNum);
				characterCount.AddRange (charsForAns1);
				lineNum = lineNum+1;				
			}

		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.48f), Screen.width * (.6f), Screen.height * (.15f)), ans2, answerStyle))
			{
				// option 1
				charsForAns2 = parser.getCharactersForAns2(lineNum);
				characterCount.AddRange (charsForAns2);
				lineNum = lineNum+1;
			}

		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.15f)), ans3, answerStyle))
			{
				// option 2
				charsForAns3 = parser.getCharactersForAns3(lineNum);
				characterCount.AddRange (charsForAns3);
				lineNum = lineNum+1;
			}
		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.82f), Screen.width * (.6f), Screen.height * (.15f)), ans4, answerStyle))
		{
			// option 3
				charsForAns4 = parser.getCharactersForAns4(lineNum);
				characterCount.AddRange (charsForAns4);
				lineNum = lineNum+1;
		}
		}
	}



}
