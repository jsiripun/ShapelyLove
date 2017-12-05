using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;

public class DialogueParser : MonoBehaviour {

	public string fileName;
	List <string> namesInDialogue;
	Dictionary<string, List<Sprite>> spritesInDialogue;

	List<DialogueLine> lines;
	Sprite mystery;

	struct DialogueLine
	{
		public string charName;
		public string content;
		public string pose;
		public string position;
		public int lineJump;
		public int relationshipNum;

		public DialogueLine(string n, string c, string p, string pos, int lj, int relnum)
		{
			charName = n;
			content = c;
			pose = p;
			position = pos;
			lineJump = lj;
			relationshipNum = relnum;
		}
	}

	// Use this for initialization
	void Start () {
		lines = new List<DialogueLine>();
		namesInDialogue = new List<string> ();
		spritesInDialogue = new Dictionary<string, List<Sprite>> ();

	}

	// Update is called once per frame
	void Update () {

	}

	public void reloadDialogue()
	{
		// clear up old dialogue
		lines.Clear ();
		namesInDialogue.Clear ();
		spritesInDialogue.Clear ();

		// relead everything with new
		LoadDialogue(fileName);
		LoadImages();
	}

	public int GetLineCount()
	{
		return lines.Count;
	}

	public string GetName(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].charName;

		return "";
	}

	public string GetContent(int lineNumber)
	{
		if(lineNumber < lines.Count)
			return lines[lineNumber].content;

		return "";
	}

	public Sprite GetPose(int lineNumber)
	{
		string temp = GetName(lineNumber);
		List<Sprite> tempSpriteArray = spritesInDialogue [temp];
		if (lineNumber < lines.Count)
			return tempSpriteArray[int.Parse(lines[lineNumber].pose)];


		return null;
	}

	public string GetPosition (int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].position;

		return "";
	}

	public int GetLineJump(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].lineJump;

		return 0;
	}

	public string GetOptionZero(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].charName;

		return "";
	}

	public string GetOptionOne(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].content;

		return "";
	}

	public string GetOptionTwo(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].pose;

		return "";
	}

	public int GetRelationshipNum(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].relationshipNum;

		return 0;
	}

	void LoadDialogue(string filename)
	{
		string file = "Assets/Resources/Dialogue/" + filename;
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
						string[] line_values = SplitCsvLine(line);

						DialogueLine line_entry = new DialogueLine(line_values[0], line_values[1], line_values[2], line_values[3], int.Parse(line_values[4]), int.Parse(line_values[5]));

						// add to names in dialogue for loading sprite purpose
						if(!namesInDialogue.Contains(line_values[0]) && line_values[3]!="S" && line_values[3]!="Q") {
							string tempName = line_values[0];
							namesInDialogue.Add(tempName); 
							List<Sprite> tempSpriteList = new List<Sprite>();
							spritesInDialogue.Add(tempName, tempSpriteList);
						}

						lines.Add(line_entry);

					} catch (Exception e) {
						Debug.Log("ERROR in dialogue line number " + (lines.Count));
					}

				}
			} while (line != null);

			r.Close();
		}
	}

	string[] SplitCsvLine(string line)
	{
		string pattern = @"
     # Match one value in valid CSV string.
     (?!\s*$)                                      # Don't match empty last value.
     \s*                                           # Strip whitespace before value.
     (?:                                           # Group for value alternatives.
       '(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
     | ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
     | (?<val>[^,'""\s\\]*(?:\s+[^,'""\s\\]+)*)    # or $3: Non-comma, non-quote stuff.
     )                                             # End group of value alternatives.
     \s*                                           # Strip whitespace after value.
     (?:,|$)                                       # Field ends on comma or EOS.
     ";

		string[] values = (from Match m in Regex.Matches(line, pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
			select m.Groups[1].Value).ToArray();

		return values;
	}

	void LoadImages()
	{
		foreach (var x in namesInDialogue) {
			try {

				var sprites = Resources.LoadAll("Sprites/Characters/" + x + "/", typeof(Sprite)).Cast<Sprite>();
				List<Sprite> temp = spritesInDialogue[x];
				foreach (var s in sprites)
				{
					temp.Add(s);
				}

			} catch (Exception e) {
				Debug.Log ("Failed to load sprites: " + x);
			}
		}
	}

}