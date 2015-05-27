using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Conversation;
using JSON;

public abstract class Personality : MonoBehaviour
{
	
	public class Word
	{
		
		private string word;
		private float[] syllables;
		private float rest;

		public Word (string word, float[] syllables, float rest)
		{
			this.word = word;
			this.syllables = syllables;
			this.rest = rest;
		}
		
		public string GetWord ()
		{
			return word;
		}
		
		public float[] GetSyllables ()
		{
			return syllables;
		}
		
		public float GetSyllable (int i)
		{
			return syllables [i];
		}
		
		public float GetRest ()
		{
			return rest;
		}
		
	}
	
	public class Phrase
	{
		
		private Word[] phrase;

		public Phrase (Word[] phrase)
		{
			this.phrase = phrase;
		}
		
		public Word[] GetPhrase ()
		{
			return phrase;
		}
		
		public Word GetWord (int i)
		{
			return phrase [i];
		}
		
	}

	public Interaction interaction;
	private Image bubbleImage;
	private Text bubbleText;
	private Animator animator;
	protected Phrase nextPhrase = null;

	void Start ()
	{
		animator = GetComponent<Animator> ();
	}
	
	void Update ()
	{
		if (nextPhrase != null) {
			StopCoroutine("Speak");
			StartCoroutine ("Speak", nextPhrase);
			nextPhrase = null;
		}
	}

	public abstract Phrase PhraseFor (Statement statement, ConversationContext context);

	public abstract string CharacterName ();

	public abstract void HandleWrongAnswer(Statement statement, JSONObject response, ConversationContext context);

	public void SpeakPhraseFor (Statement statement, ConversationContext context)
	{
		nextPhrase = PhraseFor (statement, context);
	}

	IEnumerator Speak (Phrase phrase)
	{
		Text text = interaction.AddSpeechText ();
		text.text = CharacterName () + ":";
		foreach (Word word in phrase.GetPhrase ()) {
			foreach(float syllable in word.GetSyllables ()) {
				text.text = text.text + " " + word.GetWord ();
				animator.SetTrigger ("talk");
				yield return new WaitForSeconds(syllable);
			}
			yield return new WaitForSeconds(word.GetRest ());
		}
	}

}