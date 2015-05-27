using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//using System.IO.Pipes;
using System.Text;
using System.Threading;
using JSON;

public class PipeSpeech : MonoBehaviour
{
	/*

	private static NamedPipeServerStream pipe;
	private static UnicodeEncoding streamEncoding;
	private static Thread server;
	private static bool running = false;
	private static LinkedList<PipeSpeech> instances = new LinkedList<PipeSpeech> ();
	
	public static void Serve ()
	{
		pipe = 
			new NamedPipeServerStream ("SpeechToUnityPipe", PipeDirection.InOut, 2);
		
		pipe.WaitForConnection ();

		streamEncoding = new UnicodeEncoding ();

		try {
			byte[] outBuffer = streamEncoding.GetBytes("I Can - The Yak - Unity3d\n" + "waiting on speech data");
			int len = outBuffer.Length;
			pipe.WriteByte((byte)(len / 256));
			pipe.WriteByte((byte)(len & 255));
			pipe.Write(outBuffer, 0, len);
			pipe.Flush();
			while (true) {
				len = pipe.ReadByte() * 256;
				len += pipe.ReadByte();
				byte[] inBuffer = new byte[len];
				pipe.Read(inBuffer, 0, len);
				string message = streamEncoding.GetString(inBuffer);

				outBuffer = streamEncoding.GetBytes("Message Recieved");
				len = outBuffer.Length;
				pipe.WriteByte((byte)(len / 256));
				pipe.WriteByte((byte)(len & 255));
				pipe.Write(outBuffer, 0, len);
				pipe.Flush();
			}
		} catch (IOException e) {
			Debug.Log (e.ToString ());
		}

		pipe.Close ();
	}
	
	public static void Shutdown ()
	{
		running = false;
	}
	
	public void InstanceShutdown ()
	{
		//Debug.Log ("Speech.InstanceShutdown()");
		Shutdown ();
	}
	
	public Menu menu;
	public Interaction interaction;

	void Start ()
	{
		instances.AddLast (this);
		//Debug.Log ("starting server");
		server = new Thread (Serve);
		server.Start ();
		interaction.Ping ();
		try {
			System.Diagnostics.Process src = new System.Diagnostics.Process ();
			src.StartInfo.CreateNoWindow = true;
			src.StartInfo.FileName = "src.exe";
			//src.StartInfo.UseShellExecute = false;
			src.Start ();
		} catch (Exception e) {
			Debug.Log (e.ToString ());
		}
	}
	
	public void HandleMessage (string msg)
	{
		JSONObject message = JSONParser.parseString (msg.Substring (msg.IndexOf ("{")));
		Debug.Log (message.ToString ());
		string type = message.getString ("event");
		Debug.Log (type);
		try {
			if (type.Equals ("LoadGrammarCompleted")) {
				//Debug.Log ("Grammar has been loaded");
			} else if (type.Equals ("SpeechDetected")) {
				interaction.HandleResponse (message);
			} else if (type.Equals ("SpeechHypothesized")) {
				interaction.Hypothesize (message.getString ("text"));
			} else if (type.Equals ("SpeechRecognized")) {
				JSONObject semantics = message.getJSON ("semantics");
				if (semantics.hasKey ("type")) {
					if (semantics.getString ("type").Equals ("response")) {
						interaction.HandleResponse (message);
					} else if (semantics.getString ("type").Equals ("command")) {
						menu.HandleCommand (semantics.getString ("command"));
						interaction.Hypothesize ("");
					}
				} else
					interaction.HandleResponse (message);
			}
		} catch (Exception e) {
			Debug.Log (e.ToString ());
		}
	}
	
	*/
}