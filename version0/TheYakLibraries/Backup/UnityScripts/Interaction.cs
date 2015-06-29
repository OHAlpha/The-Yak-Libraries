using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JSON;

public class Interaction : MonoBehaviour
{

	private RectTransform promptRect;
	private GameObject[] promptPanes;
	private Image[] promptImages;
	private Text[] promptTexts;
	private int numPromptsActive;
	private RectTransform speechRect;
	private Queue<Text> speechTexts = new Queue<Text> ();
	private Text speechText;
	private Text hypothesizedText;
	private string speech = "";
	private Queue<string> recognized = new Queue<string> ();
	private string[] prompts = new string[0];
	private CharacterScript[] scripts;

	void Start ()
	{
		foreach (RectTransform rect in GetComponentsInChildren<RectTransform> ())
			if (rect.name.Equals ("PromptPane"))
				promptRect = rect;
			else if (rect.name.Equals ("Speech"))
				speechRect = rect;
		foreach (Text text in GetComponentsInChildren<Text> ())
			if (text.name.Equals ("HypothesizedText"))
				hypothesizedText = text;
			else if (text.name.Equals ("SpeechText"))
				speechText = text;
		promptPanes = new GameObject[4];
		foreach (RectTransform rect in promptRect.GetComponentsInChildren<RectTransform> ())
			if (rect.name.Equals ("PromptAPane"))
				promptPanes [0] = rect.gameObject;
			else if (rect.name.Equals ("PromptBPane"))
				promptPanes [1] = rect.gameObject;
			else if (rect.name.Equals ("PromptCPane"))
				promptPanes [2] = rect.gameObject;
			else if (rect.name.Equals ("PromptDPane"))
				promptPanes [3] = rect.gameObject;
		promptImages = new Image[4];
		foreach (Image image in promptRect.GetComponentsInChildren<Image> ())
			if (image.name.Equals ("PromptAImage"))
				promptImages [0] = image;
			else if (image.name.Equals ("PromptBImage"))
				promptImages [1] = image;
			else if (image.name.Equals ("PromptCImage"))
				promptImages [2] = image;
			else if (image.name.Equals ("PromptDImage"))
				promptImages [3] = image;
		promptTexts = new Text[4];
		foreach (Text text in promptRect.GetComponentsInChildren<Text> ())
			if (text.name.Equals ("PromptAText"))
				promptTexts [0] = text;
			else if (text.name.Equals ("PromptBText"))
				promptTexts [1] = text;
			else if (text.name.Equals ("PromptCText"))
				promptTexts [2] = text;
			else if (text.name.Equals ("PromptDText"))
				promptTexts [3] = text;
		scripts = GetComponentsInChildren<CharacterScript> ();
		SetNumPromptsActive (0);
		scripts [0].ToStatement (0);
	}
	
	public int GetNumPromptsActive ()
	{
		return numPromptsActive;
	}
	
	public void SetNumPromptsActive (int n)
	{
		if (n > 4)
			n = 4;
		numPromptsActive = n;
		for (int i = 0; i < numPromptsActive; i++)
			promptPanes [i].SetActive (true);
		for (int i = numPromptsActive; i < 4; i++)
			promptPanes [i].SetActive (false);
		Vector2 sd = promptRect.sizeDelta;
		sd.y = numPromptsActive * 25 + 10;
		promptRect.sizeDelta = sd;
	}
	
	public void SetPrompts (string[] prompts)
	{
		this.prompts = prompts;
	}
	
	private void SetPromptsPrivate (string[] prompts)
	{
		int n = prompts.Length;
		if (n > 4)
			n = 4;
		SetNumPromptsActive (n);
		for (int i = 0; i < n; i++) {
			promptTexts[i].text = prompts[i];
		}
	}

	public void Ping ()
	{
		Debug.Log ("Interaction.cs");
	}
	
	public void Stop ()
	{
		Debug.Log ("Interaction.Stop()");
	}

	public Text AddSpeechText ()
	{
		GameObject go = (GameObject) Instantiate (speechText.gameObject);
		go.transform.SetParent (speechRect);
		Text text = go.GetComponent<Text> ();
		text.enabled = true;
		speechTexts.Enqueue (text);
		while (speechTexts.Count > 5)
			Destroy(speechTexts.Dequeue ().gameObject);
		int n = speechTexts.Count - 1;
		foreach (Text t in speechTexts) {
			RectTransform trans = t.GetComponent<RectTransform>();
			trans.anchoredPosition = new Vector3(0, 5 + 20 * n, 0);
			trans.sizeDelta = new Vector2(speechRect.sizeDelta.x-10, 20);
			n--;
		}
		return text;
	}

	public void Hypothesize (string text)
	{
		speech = text;
	}

	public void HandleResponse(JSONObject response)
	{
		foreach (CharacterScript script in scripts)
			script.HandleResponse(response);
		if (!response.GetString("event").Equals("SpeechDetected"))
			recognized.Enqueue (response.GetString ("text"));
	}

	public void Update ()
	{
		if (!speech.Equals (hypothesizedText.text))
			try {
				hypothesizedText.text = speech;
				//Debug.Log ("hypothesizedText.text = " + speech + ";");
			} catch (Exception e) {
				Debug.Log (e.ToString ());
			}
		while (recognized.Count > 0) {
			Text text = AddSpeechText ();
			text.text = "You: " + recognized.Dequeue ();
			text.color = new Color(1f,0f,0f);
		}
		if (prompts.Length > 0) {
			SetPromptsPrivate (prompts);
			prompts = new string[0];
		}
	}
}