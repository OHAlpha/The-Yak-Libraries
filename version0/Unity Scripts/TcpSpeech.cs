using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using JSON;
using TCPConnectors;

public class TcpSpeech : MonoBehaviour
{

	private TCPConnector connector;
	public Menu menu;
	public Interaction interaction;
	
	public void InstanceShutdown ()
	{
		connector.Log ("Speech.InstanceShutdown()");
		connector.Shutdown ();
	}

	void Start ()
	{
		connector = new TCPConnector(200, 201);
		connector.logger += Debug.Log;
		connector.requestHandler += HandleMessage;
		connector.commandHandler += LogMessage;
		connector.messageHandler += LogMessage;
		//connector.requestHandler += LogMessage;
		connector.Log ("starting server");
		interaction.Ping ();
		try {
			System.Diagnostics.Process src = new System.Diagnostics.Process ();
			src.StartInfo.CreateNoWindow = true;
			src.StartInfo.FileName = "RecognitionServer.exe";
			//src.StartInfo.UseShellExecute = false;
			src.Start ();
		} catch (Exception e) {
			connector.Log (e.ToString ());
		}
		connector.Start ();
		JSONObject msg = new JSONObject ();
		msg.AddString ("type", "message");
		msg.AddString ("message", "All is Well");
		connector.Message (msg,false);
	}

	void Update()
	{
		//connector.Log ("regular: "+connector.Message(new JSONObject(new string[] {"requestType", "message"}, new object[] {"message", "Nothing To Report"}),false).ToString());
	}
	
	void HandleMessage (JSONObject message, JSONObject response)
	{
		connector.Log ("handling message: "+message.ToString ());
		string type = message.GetString ("type");
		connector.Log ("message type: "+type);
		if (!type.Equals ("SpeechEvent")) {
			response.AddString ("responseType", "message");
			response.AddString ("message", "Message Received");
			return;
		}
		string eventType = message.GetString ("event");
		connector.Log ("event type: "+eventType);
		connector.Log ("should end");
		try {
			/*
			if (eventType.Equals ("LoadGrammarCompleted")) {
				connector.Log ("Grammar has been loaded");
			} else if (eventType.Equals ("SpeechDetected")) {
				interaction.HandleResponse (message);
			} else if (eventType.Equals ("SpeechHypothesized")) {
				interaction.Hypothesize (message.GetString ("text"));
			} else if (eventType.Equals ("SpeechRecognized")) {
				JSONObject semantics = message.GetJSON ("semantics");
				if (semantics.hasKey ("type")) {
					if (semantics.GetString ("type").Equals ("response")) {
						interaction.HandleResponse (message);
					} else if (semantics.GetString ("type").Equals ("command")) {
						menu.HandleCommand (semantics.GetString ("command"));
						interaction.Hypothesize ("");
					}
				} else
					interaction.HandleResponse (message);
			}
			*/
			response.AddString ("responseType", "message");
			response.AddString ("message", "Message Received");
		} catch (Exception e) {
			connector.Log ("error handling message");
			connector.Log (e.ToString ());
			response.AddString ("responseType", "message");
			response.AddString ("message", "Error Processing");
			response.AddString ("error", e.ToString ());
		}
		connector.Log ("HandleMessage End\n");
	}
	
	void LogMessage(string str, JSONObject response)
	{
		Debug.Log (str);
	}
	
	void LogMessage(JSONObject obj, JSONObject response)
	{
		Debug.Log (obj.ToString ());
	}

}