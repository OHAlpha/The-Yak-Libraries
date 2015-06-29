using UnityEngine;
using System.Collections;
using Conversation;
using JSON;

public class FirstScript : CharacterScript
{

	void Awake ()
	{
		SetStatements ();
	}

	void Update ()
	{
	
	}

	override
	public Statement[] CreateStatements ()
	{
		Statement[] statements = new Statement[] {
			new SimpleStatement ("Hello. My name is Bear. What is your name?",
		                                     "Question.What.Name", 0,
			                     Statement.SENTENCE_TYPE_INTEROGATIVE, "What", "is", "your name",new string[] {}),
			new SimpleStatement ("I've never heard that name before. Say, how old are you?",
		                                     "Question.What.Age", 0,
			                     Statement.SENTENCE_TYPE_INTEROGATIVE, "What", "is", "your age",new string[] {}),
			new SimpleStatement ("Wow! So. Do you like to play outside?",
			                     "Question.YN", 0,
			                     Statement.SENTENCE_TYPE_INTEROGATIVE, "You", "like", "playing outside",new string[] {"Yes","No"}),
			new SimpleStatement ("Cool! Me too! What things do you like to do outside?",
			                     "Question.What", 0,
			                     Statement.SENTENCE_TYPE_INTEROGATIVE, "", "", "",new string[] {"Fishing","Swimming","Playing","Catching"}),
			new SimpleStatement ("Awesome! Where is your favorite place to play?",
			                     "Question.Where", 0,
			                     Statement.SENTENCE_TYPE_INTEROGATIVE, "", "", "",new string[] {"Park","Friend's house","Water park","Skating Rink"}),
			new SimpleStatement ("What kind of things do you like to do inside?",
			                     "Question.What", 0,
			                     Statement.SENTENCE_TYPE_INTEROGATIVE, "", "", "",new string[] {"Watch TV","Play board games","Do puzzles","Use my tablet"}),
			new SimpleStatement ("Nice! Well it was nice talking to you. Goodbye!",
			                     "Remark.Bye", 0,
			                     Statement.SENTENCE_TYPE_DECLARATIVE, "", "", "",new string[] {"Goodbye"})
		};
		/*
I enjoy fishing, what kind of activities do you enjoy outside? (fishing, swimming, playing, catching, biking)***
What do you see outside when you are having fun? (birds, planes, bugs, grass, clouds, cars)***
Where is your favorite place to go play? (park, friend’s house, water park, skating rink)***
What do you do inside when it’s a rainy day? (Watch tv, play board games, puzzles, use tablet)***
What kind of sports do you enjoy playing? (Soccer, baseball, basketball, hockey, football)***
What do you wanna be when you grow up? ( Doctor, policeman, fireman, medic, nurse, scientist, salesman, engineer)***
		*/
		return statements;
	}
	
	override
		public int PlexResponse (JSONObject response)
	{
		if (state == 0) {
			if (!response.GetString ("event").Equals ("SpeechDetected")) {
				if (response.GetJSON ("semantics").GetString ("type").Equals ("name"))
					context.AddValue ("Player Name", response.GetString ("text"));
			}
			return 1;
		} else if (state == 1) {
			if (response.GetString ("event").Equals ("SpeechRecognized")) {
				if (!response.GetJSON ("semantics").GetString ("type").Equals ("response"))
					return STATE_IGNORE;
				else if (response.GetJSON ("semantics").GetString ("responsetype").Equals ("ageresponse")) {
					context.AddValue ("Player Age", response.GetJSON ("semantics").GetString ("value"));
					return 2;
				} else
					return STATE_WRONG;
			}
		} else if (state == 2) {
			if (response.GetString ("event").Equals ("SpeechRecognized")) {
				if (!response.GetJSON ("semantics").GetString ("type").Equals ("response"))
					return STATE_IGNORE;
				else if (response.GetJSON ("semantics").GetString ("responsetype").Equals ("yesno")) {
					if(response.GetJSON("semantics").GetString ("value").Equals("no"))
						return 5;
					else
						return 3;
				}
				else
					return STATE_WRONG;
			}
		} else if (state == 3) {
			if (response.GetString ("event").Equals ("SpeechRecognized")) {
				if (!response.GetJSON ("semantics").GetString ("type").Equals ("response"))
					return STATE_IGNORE;
				else if (response.GetJSON ("semantics").GetString ("responsetype").Equals ("activity")) {
					return 4;
				} else
					return STATE_WRONG;
			}
		} else if (state == 4) {
			if (response.GetString ("event").Equals ("SpeechRecognized")) {
				if (!response.GetJSON ("semantics").GetString ("type").Equals ("response"))
					return STATE_IGNORE;
				else if (response.GetJSON ("semantics").GetString ("responsetype").Equals ("placestogo")) {
					return 6;
				} else
					return STATE_WRONG;
			}
		} else if (state == 5) {
			if (response.GetString ("event").Equals ("SpeechRecognized")) {
				if (!response.GetJSON ("semantics").GetString ("type").Equals ("response"))
					return STATE_IGNORE;
				else if (response.GetJSON ("semantics").GetString ("responsetype").Equals ("insideactivity")) {
					return 6;
				} else
					return STATE_WRONG;
			}
		} else if (state == 6) {
			return STATE_FINAL;
		}
		return STATE_IGNORE;
	}
	
}