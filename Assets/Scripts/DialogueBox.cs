using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueBox : MonoBehaviour {

	DialogueParser parser;

	public string dialogue;
	public string charName;
	public Sprite pose;
	public string position;
	public int lineJump;
	public int relNum;

	// for question options
	private string optionZero;
	private string optionOne;
	private string optionTwo;
	private string questionOption;

	//player stats
	public static Player playa;

	// for current text display
	private string currText;
	private float letterPause = 0.05f;
	private bool stillRunning;
	private IEnumerator textCoroutine;

	bool clickedDialogue;
	bool clickedQuestion;
	int lineNum;

	public GUIStyle customStyle, customStyleName, questionStyle, answerStyle;

	// Use this for initialization
	void Start () {
		dialogue = "";
		parser = GameObject.Find("DialogueParserObject").GetComponent<DialogueParser>();
		playa = GameObject.Find("Player").GetComponent<Player>();
		parser.fileName = playa.getCurrentDialogue();
		parser.reloadDialogue();
		lineNum = 0;
		clickedDialogue = false;
		clickedQuestion = false;
		currText = "";
		stillRunning = false;
		textCoroutine = TypeText();
		plainDisplay();
	}


	// Update is called once per frame
	void Update () {

		setBackground();

		if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
		{
			position = parser.GetPosition(lineNum);
			dialogue = replaceWithPlayerName(parser.GetContent(lineNum));

			if (position == "Q")
			{
				// question
				clickedDialogue = false;
				clickedQuestion = true;
			}
			else if (position == "S")
			{
				// load next scene
				clickedDialogue = false;
				charName = parser.GetName(lineNum);
				dialogue = replaceWithPlayerName(parser.GetContent(lineNum));
				StartCoroutine(ChangeScene(charName, dialogue));
				return;
			}
			else if (stillRunning)
			{
				StopCoroutine(textCoroutine);
				currText = replaceWithPlayerName(dialogue);
				stillRunning = false;

				lineNum = lineNum + 1 + lineJump;
			}
			else
			{
				plainDisplay();
			}
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

	void plainDisplay()
	{
		ResetImages();
		clickedDialogue = true;
		currText = "";

		charName = parser.GetName(lineNum);
		dialogue = replaceWithPlayerName(parser.GetContent(lineNum));
		pose = parser.GetPose(lineNum);
		position = parser.GetPosition(lineNum);
		lineJump = parser.GetLineJump(lineNum);
		relNum = parser.GetRelationshipNum(lineNum);

		// add relationship points
		playa.addRelationshipNum(charName, relNum);  

		DisplayImages();
		textCoroutine = TypeText();
		StartCoroutine(textCoroutine);


		if (lineNum > parser.GetLineCount())
			clickedDialogue = false;


	}

	IEnumerator TypeText()
	{
		stillRunning = true;
		foreach(char letter in dialogue.ToCharArray())
		{
			currText += letter;
			yield return new WaitForSeconds(letterPause);
		}
		lineNum = lineNum + 1 + lineJump;
		stillRunning = false;
	}

	IEnumerator ChangeScene(string sceneName, string sceneDialogue)
	{
		playa.loadNextDialogue(sceneDialogue);
		playa.addClassPoint (sceneName, 1);
		float fadeTime = GameObject.Find ("DialogueBoxObject").GetComponent<Fading>().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene(sceneName);

	}


	void OnGUI()
	{
		customStyle.fontSize = Mathf.RoundToInt(((Screen.width * .2f) + (Screen.height * .32f)) / 13f);
		customStyleName.fontSize = Mathf.RoundToInt(((Screen.width * .25f) + (Screen.height * .1f)) / 10f);
		questionStyle.fontSize = Mathf.RoundToInt(((Screen.width * .2f) + (Screen.height * .32f)) / 10f);
		answerStyle.fontSize = Mathf.RoundToInt(((Screen.width * .2f) + (Screen.height * .32f)) / 15f);
		// set word wrap for all
		questionStyle.wordWrap = true;
		answerStyle.wordWrap = true;
		customStyle.wordWrap = true;
		customStyleName.wordWrap = true;

		// set padding
		customStyle.padding.left = 20;
		customStyle.padding.top = 10;
		customStyle.padding.right = 20;
		customStyle.padding.bottom = 10;

		questionStyle.padding.left = 20;
		questionStyle.padding.top = 10;
		questionStyle.padding.right = 20;
		questionStyle.padding.bottom = 10;

		answerStyle.padding.left = 10;
		answerStyle.padding.top = 5;
		answerStyle.padding.right = 10;
		answerStyle.padding.bottom = 5;

		if (clickedDialogue)
		{
			GUI.FocusControl(null);
			GUI.TextField(new Rect(Screen.width * (.1f), Screen.height * (.7f), Screen.width * (.8f), Screen.height * (.3f)), currText, customStyle);
			if (charName == "Player") {
				string playerName = playa.getPlayerName ();
				GUI.TextField (new Rect (Screen.width * (0), Screen.height * (.6f), Screen.width * (.25f), Screen.height * (.1f)), playerName, customStyleName);
			} else {
				GUI.TextField (new Rect (Screen.width * (0), Screen.height * (.6f), Screen.width * (.25f), Screen.height * (.1f)), charName, customStyleName);
			}
		}

		if(clickedQuestion)
		{
			optionZero = parser.GetOptionZero(lineNum);
			optionOne = parser.GetOptionOne(lineNum);
			optionTwo = parser.GetOptionTwo(lineNum);
			lineJump = parser.GetLineJump(lineNum);
			questionOption = replaceWithPlayerName(parser.GetContent(lineNum - 1));

			GUI.Label(new Rect(Screen.width * (.1f), Screen.height * (0), Screen.width * (.8f), Screen.height * (.3f)), questionOption, questionStyle);

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.32f), Screen.width * (.6f), Screen.height * (.2f)), optionZero, answerStyle))
			{
				// option 0
				lineNum = lineNum + 1;
				clickedQuestion = false;
				clickedDialogue = true;
				plainDisplay();
			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.53f), Screen.width * (.6f), Screen.height * (.2f)), optionOne, answerStyle))
			{
				// option 1
				lineNum = lineNum + 2;
				clickedQuestion = false;
				clickedDialogue = true;
				plainDisplay();
			}

			if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.74f), Screen.width * (.6f), Screen.height * (.2f)), optionTwo, answerStyle))
			{
				// option 2
				lineNum = lineNum + 3;
				clickedQuestion = false;
				clickedDialogue = true;
				plainDisplay();
			}
		}
	}


	void ResetImages()
	{
		if(charName != "")
		{
			GameObject character = GameObject.Find("CharacterSprite");
			SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
			currSprite.sprite = null;
		}
	}

	void DisplayImages()
	{
		if(charName == "Mystery")
		{
			GameObject character = GameObject.Find("Mystery");

			SetSpritePositions(character);

			SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
			currSprite.sprite = pose;
		}
		else if(charName != "")
		{
			GameObject character = GameObject.Find("CharacterSprite");

			SetSpritePositions(character);

			SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
			currSprite.sprite = pose;
		}
	}

	void SetSpritePositions(GameObject spriteObj)
	{
		if(position == "L" || charName == "Player")
		{
			spriteObj.transform.position = new Vector3(0, 2, -3);
		}
		else
		{
			spriteObj.transform.position = new Vector3(0, 2, -3);
		}
	}

	private string replaceWithPlayerName(string toCheckAndReplace){
		return toCheckAndReplace.Replace ("[PLAYERNAME]", playa.getPlayerName ());;
	}
}