using System.Collections;
using JSON;

namespace Conversation
{
	public abstract class Statement
	{
		
		public static int SENTENCE_TYPE_DECLARATIVE = 0;
		public static int SENTENCE_TYPE_INTEROGATIVE = 1;
		public static int SENTENCE_TYPE_IMPERATIVE = 2;
		
		public abstract string[] GetSimplePhrase ();
		
		public abstract string GetStatementType ();

		public abstract int GetPersonality ();
		
		public abstract int SentenceType ();
		
		public abstract string Subject ();
		
		public abstract string Verb ();
		
		public abstract string Object ();

		public abstract string[] GetPrompts();
		
	}
	
	public class SimpleStatement : Statement
	{
		
		private string phrase;
		private string type;
		private int character;
		private int sentence;
		private string subject;
		private string verb;
		private string obj;
		private string[] prompts;
		
		public SimpleStatement (string phrase, string type, int character, int sentence, string subject, string verb, string obj, string[] prompts)
		{
			this.phrase = phrase;
			this.type = type;
			this.character = character;
			this.sentence = sentence;
			this.subject = subject;
			this.verb = verb;
			this.obj = obj;
			this.prompts = prompts;
		}
		
		override
			public string[] GetSimplePhrase ()
		{
			return phrase.Split (new char[] {' '});
		}
		
		override
			public string GetStatementType ()
		{
			return type;
		}
		
		override
			public int GetPersonality ()
		{
			return character;
		}
		
		override
			public int SentenceType ()
		{
			return sentence;
		}
		
		override
			public string Subject ()
		{
			return subject;
		}
		
		override
			public string Verb ()
		{
			return verb;
		}
		
		override
			public string Object ()
		{
			return obj;
		}
		
		override
			public string[] GetPrompts ()
		{
			return prompts;
		}
		
	}

}