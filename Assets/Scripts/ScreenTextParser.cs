using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;

public class ScreenTextParser : MonoBehaviour {
	List<string> textLines;
	public string fileName;
	// Use this for initialization
	void Start () {
		textLines = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	List<string> getTextLines(){
		return textLines;
	}

	public int GetLineCount()
	{
		return textLines.Count;
	}

	public string getScreenTextAt(int lineNumber) 
	{
		if (lineNumber < textLines.Count)
			return textLines [lineNumber];

		return null;
	}

	public void reloadScreenText()
	{
		// clear up any old questions, should not be any
		textLines.Clear ();

		// relead everything with new
		LoadText(fileName);
	}

	void LoadText(string filename)
	{
		string file = "Assets/Resources/DisplayText/" + filename;
		string line;
		StreamReader r = new StreamReader(file);

		using (r)
		{
			do
			{
				line = r.ReadLine();
				if (line != null)
				{
					try {
						textLines.Add(line);

					} catch (Exception e) {
						Debug.Log("ERROR in dialogue line number " + (textLines.Count));
					}

				}
			} while (line != null);

			r.Close();
		}
	}

}
