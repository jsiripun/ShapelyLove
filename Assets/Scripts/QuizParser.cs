using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;

public class QuizParser : MonoBehaviour {

	public string fileName;

	List<QuestionLine> lines = new List<QuestionLine>();

	struct QuestionLine
	{
		public string question;
		public string answer1;
		public string answer2;
		public string answer3;
		public string answer4;
		public int numAnswers;
		public List<string> charactersForAns1;
		public List<string> charactersForAns2;
		public List<string> charactersForAns3;
		public List<string> charactersForAns4;

		public QuestionLine(string q, string a1, string a2, string a3, string a4, int numAns, List<string> chForA1, List<string> chForA2, List<string> chForA3, List<string> chForA4)
		{
			question = q;
			answer1 = a1;
			answer2 = a2;
			answer3 = a3;
			answer4 = a4;
			numAnswers = numAns;
			charactersForAns1 = chForA1;
			charactersForAns2 = chForA2;
			charactersForAns3 = chForA3;
			charactersForAns4 = chForA4;
		}

		public void setQuestion(string q) 
		{
			question = q;
		}

		public void setAnswer1(string a1) 
		{
			answer1 = a1;
		}

		public void setAnswer2(string a2) 
		{
			answer2 = a2;
		}

		public void setAnswer3(string a3) 
		{
			answer3 = a3;
		}

		public void setAnswer4(string a4) 
		{
			answer4 = a4;
		}

		public void setNumAnswers(int numAns) 
		{
			numAnswers = numAns;
		}

		public string getQuestion()
		{
			return question;
		}

		public string getAnswer1()
		{
			return answer1;
		}

		public string getAnswer2()
		{
			return answer2;
		}

		public string getAnswer3()
		{
			return answer3;
		}

		public string getAnswer4()
		{
			return answer4;
		}

		public int getNumAnswers()
		{
			return numAnswers;
		}
	}


	public int GetLineCount()
	{
		return lines.Count;
	}


	public string getQuestion(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].question;

		return "";
	}

	public string getAnswer1(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].answer1;

		return "";
	}

	public string getAnswer2(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].answer2;

		return "";
	}

	public string getAnswer3(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].answer3;

		return "";
	}

	public string getAnswer4(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].answer4;

		return "";
	}

	public int getNumAnswers(int lineNumber)
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].numAnswers;

		return 0;
	}

	public List<string> getCharactersForAns1(int lineNumber) 
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].charactersForAns1;

		return null;
	}

	public List<string> getCharactersForAns2(int lineNumber) 
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].charactersForAns2;

		return null;
	}

	public List<string> getCharactersForAns3(int lineNumber) 
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].charactersForAns3;

		return null;
	}

	public List<string> getCharactersForAns4(int lineNumber) 
	{
		if (lineNumber < lines.Count)
			return lines[lineNumber].charactersForAns4;

		return null;
	}

	public void loadQuestions()
	{
		// clear up any old questions, should not be any
		lines.Clear ();

		// relead everything with new
		LoadQuestions(fileName);
	}

	private void LoadQuestions(string filename)
	{
		string file = "Assets/Resources/Quiz/" + filename;
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

						QuestionLine line_entry = new QuestionLine();
						line_entry.setQuestion(line_values[0]);

						if(line_values.Length == 3) {
							List<string> tempAns1 = getShapeResults(line_values[1]);
							line_entry.setAnswer1(tempAns1.ElementAt(0));
							tempAns1.RemoveAt(0);
							line_entry.charactersForAns1 = tempAns1;

							List<string> tempAns2 = getShapeResults(line_values[2]);
							line_entry.setAnswer2(tempAns2.ElementAt(0));
							tempAns2.RemoveAt(0);
							line_entry.charactersForAns2 = tempAns2;

							line_entry.setNumAnswers(2);
						}
						else if(line_values.Length== 4) {
							List<string> tempAns1 = getShapeResults(line_values[1]);
							line_entry.setAnswer1(tempAns1.ElementAt(0));
							tempAns1.RemoveAt(0);
							line_entry.charactersForAns1 = tempAns1;

							List<string> tempAns2 = getShapeResults(line_values[2]);
							line_entry.setAnswer2(tempAns2.ElementAt(0));
							tempAns2.RemoveAt(0);
							line_entry.charactersForAns2 = tempAns2;

							List<string> tempAns3 = getShapeResults(line_values[3]);
							line_entry.setAnswer3(tempAns3.ElementAt(0));
							tempAns3.RemoveAt(0);
							line_entry.charactersForAns3 = tempAns3;

							line_entry.setNumAnswers(3);
						}
						else if(line_values.Length == 5) {
							List<string> tempAns1 = getShapeResults(line_values[1]);
							line_entry.setAnswer1(tempAns1.ElementAt(0));
							tempAns1.RemoveAt(0);
							line_entry.charactersForAns1 = tempAns1;

							List<string> tempAns2 = getShapeResults(line_values[2]);
							line_entry.setAnswer2(tempAns2.ElementAt(0));
							tempAns2.RemoveAt(0);
							line_entry.charactersForAns2 = tempAns2;

							List<string> tempAns3 = getShapeResults(line_values[3]);
							line_entry.setAnswer3(tempAns3.ElementAt(0));
							tempAns3.RemoveAt(0);
							line_entry.charactersForAns3 = tempAns3;

							List<string> tempAns4 = getShapeResults(line_values[4]);
							line_entry.setAnswer4(tempAns4.ElementAt(0));
							tempAns4.RemoveAt(0);
							line_entry.charactersForAns4 = tempAns4;

							line_entry.setNumAnswers(4);
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

	private string[] SplitCsvLine(string line)
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

	private List<string> getShapeResults(string input) {
		char delimiter = '|';
		List<string> toReturn = input.Split (delimiter).ToList();
		return toReturn;
	}
}
