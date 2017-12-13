using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private static string playerShape = "";
	private static string playerName = "";
	public static int relationshipLvlBella = 0;
	public static string quizToLoad = "PlayerQuizQuestions.txt";
	public static string screenTextToDisplay = "QuizStartText.txt";
	public static string currentDialogueLoad = "";
	//public static string currentDialogueLoad = "DialogueAbuelaHome_Start.txt"; //change after done testing

	public void addRelationshipNum(string relName, int relNum)
	{
		if (relName == "Bella")
			relationshipLvlBella += relNum;

	}

	public void loadNextDialogue (string dialogueToLoad)
	{
		currentDialogueLoad = dialogueToLoad;
	}

	public string getCurrentDialogue()
	{
		return currentDialogueLoad;
	}

	public string getQuizQuestions()
	{
		return quizToLoad;
	}

	public string getScreenTextToDisplay()
	{
		return screenTextToDisplay;
	}

	public void setScreenTextToDisplay(string filename) {
		screenTextToDisplay = filename;
	}

	public string getPlayerShape() {
		return playerShape;
	}

	public void setPlayerShape(string shapeToSet) {
		playerShape = shapeToSet;
	}

	public void setPlayerName(string nameToSet) {
		playerName = nameToSet;
	}

	public string getPlayerName () {
		return playerName;
	}

}