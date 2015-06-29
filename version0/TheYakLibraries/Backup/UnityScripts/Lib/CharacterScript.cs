using UnityEngine;
using System.Collections;
using Conversation;
using JSON;

public abstract class CharacterScript : MonoBehaviour
{
	
	public static int STATE_FINAL = -1;
	public static int STATE_EXIT = -2;
	public static int STATE_WRONG = -3;
	public static int STATE_IGNORE = -4;
	public Interaction interaction;
	public Personality[] characters;
	protected ConversationContext context = new ConversationContext();
	protected Statement[] statements;
	protected int state = STATE_FINAL;

	public void SetStatements ()
	{
		statements = CreateStatements ();
	}

	void Update ()
	{
	
	}

	public abstract Statement[] CreateStatements ();

	public void ToStatement (int i)
	{
		state = i;
		if (state == STATE_FINAL || state == STATE_EXIT)
			return;
		Statement statement = statements [state];
		Personality personality = characters [statement.GetPersonality ()];
		context.State (personality, statement);
		personality.SpeakPhraseFor (statement, context);
		Debug.Log ("state = " + i + ". Statement is a " + statement.GetType ().Name );
		interaction.SetPrompts (statement.GetPrompts ());
	}

	public abstract int PlexResponse(JSONObject response);

	public void HandleResponse(JSONObject response)
	{
		if( state == STATE_FINAL || state == STATE_EXIT)
			return;
		Statement statement = statements [state];
		Personality personality = characters [statement.GetPersonality ()];
		int i = PlexResponse (response);
		if (i == STATE_IGNORE)
			return;
		else if (i == STATE_WRONG)
			personality.HandleWrongAnswer (statement, response, context);
		else {
			ToStatement (i);
			Debug.Log ("ToStatement( " + i + " )");
		}
	}

}