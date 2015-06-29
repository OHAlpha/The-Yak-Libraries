using System;
using System.Collections;
using System.Collections.Generic;
using JSON;

namespace Conversation
{

	public class ConversationContext
	{

		private Hashtable memory = new Hashtable();
		
		private List<Personality> personalities = new List<Personality> ();
		
		private List<Statement> statements = new List<Statement> ();
		
		private List<JSONObject> responses = new List<JSONObject> ();

		public bool HasKey( string key )
		{
			return memory.ContainsKey (key);
		}

		public Object GetValue( string key )
		{
			foreach (DictionaryEntry de in memory)
				if (de.Key.Equals (key))
					return de.Value;
			return null;
		}

		public void AddValue(string key, Object value)
		{
			memory.Add (key, value);
		}
		
		public void State( Personality personality, Statement statement )
		{
			personalities.Add (personality);
			statements.Add (statement);
		}
		
		public void Respond( JSONObject response )
		{
			responses.Add (response);
		}

	}

}