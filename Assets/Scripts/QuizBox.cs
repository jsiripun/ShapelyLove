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
		lineNum = 0;
		setBackground();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("line num: " + lineNum);
		//Debug.Log ("file count: " + fileLineCount);
		if (lineNum == fileLineCount) {
			Debug.Log ("at end of file");
		}

	}

	void setBackground()
	{
		GameObject background = GameObject.Find("background");

		SpriteRenderer sr = background.GetComponent<SpriteRenderer>();

		float worldScreenHeight = Camera.main.orthographicSize * 2;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		background.transform.localScale = new Vector3(
			worldScreenWidth / sr.sprite.bounds.size.x * 1.75f,
			worldScreenHeight / sr.sprite.bounds.size.y * 1.75f, 0);
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
				lineNum = lineNum+1;

			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.30f)), ans2, answerStyle))
			{
				// option 1
				lineNum = lineNum+1;
			}
		} else if (numAns == 3) {
			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.30f), Screen.width * (.6f), Screen.height * (.15f)), ans1, answerStyle))
			{
				// option 0
				lineNum = lineNum+1;

			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.48f), Screen.width * (.6f), Screen.height * (.15f)), ans2, answerStyle))
			{
				// option 1
				lineNum = lineNum+1;
			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.15f)), ans3, answerStyle))
			{
				// option 2
				lineNum = lineNum+1;
			}
		
		} else if (numAns == 4) {
			
		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.30f), Screen.width * (.6f), Screen.height * (.15f)), ans1, answerStyle))
			{
				// option 0
				lineNum = lineNum+1;
				
			}

		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.48f), Screen.width * (.6f), Screen.height * (.15f)), ans2, answerStyle))
			{
				// option 1
				lineNum = lineNum+1;
			}

		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.65f), Screen.width * (.6f), Screen.height * (.15f)), ans3, answerStyle))
			{
				// option 2
				lineNum = lineNum+1;
			}
		if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.82f), Screen.width * (.6f), Screen.height * (.15f)), ans4, answerStyle))
		{
			// option 2
			lineNum = lineNum+1;
		}
		}
	}



}
