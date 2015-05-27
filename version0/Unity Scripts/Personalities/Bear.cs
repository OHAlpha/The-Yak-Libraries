using UnityEngine;
using System.Collections;
using Conversation;
using JSON;

public class Bear : Personality
{

	override
		public string CharacterName ()
	{
		return "Bear";
	}

	override
		public Phrase PhraseFor (Statement statement, ConversationContext context)
	{
		string[] strWords = statement.GetSimplePhrase ();
		int n = strWords.GetLength (0);
		Word[] words = new Word[n];
		for (int i = 0; i < n; i++)
			words [i] = new Word (strWords [i], new float[] {.25f}, .2f);
		return new Phrase (words);
	}

	override
	public void HandleWrongAnswer(Statement statement, JSONObject response, ConversationContext context)
	{
		Word[] phrase = PhraseFor (statement, context).GetPhrase ();
		string[] correction = "Sorry but I don't know what you're talking about.".Split (new char[] {' '});
		Word[] words = new Word[phrase.GetLength (0) + correction.GetLength (0)];
		int n = correction.GetLength (0);
		int i;
		for (i = 0; i < n; i++)
			words [i] = new Word (correction [i], new float[] {.25f}, .2f);
		for (i = n; i < words.GetLength (0); i++)
			words [i] = phrase[i - n];
		nextPhrase = new Phrase (words);
	}

}