using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{

	private Canvas cnvMenu;
	private Button playDemo;
	private LinkedList<string> commandQueue = new LinkedList<string> ();

	void Start ()
	{
		cnvMenu = GetComponent<Canvas> ();
		Button[] buttons = GetComponentsInChildren<Button> ();
		foreach (Button b in buttons)
			if (b.name.Equals ("PlayDemoButton"))
				playDemo = b;
		Debug.Log (Application.loadedLevelName);
		if (!Application.loadedLevelName.Equals ("Menu"))
			cnvMenu.enabled = false;
		else if (Application.loadedLevelName.Equals ("Demo"))
			playDemo.interactable = false;
	}

	public void Update ()
	{
		foreach (string cmd in commandQueue)
			ExecCommand (cmd);
		commandQueue.Clear ();
	}
	
	public void HandleCommand (string cmd)
	{
		commandQueue.AddLast (cmd);
	}
	
	public void ExecCommand (string cmd)
	{
		if (cmd.Equals ("showmenu"))
			ShowMenu ();
		else if (cmd.Equals ("hidemenu"))
			HideMenu ();
		else if (cmd.Equals ("togglemenu"))
			ToggleMenu ();
		else if (cmd.Equals ("demo")) {
			if (!Application.loadedLevelName.Equals ("Demo") && cnvMenu.enabled)
				PlayDemo ();
		}
	}
	
	public void ShowMenu ()
	{
		cnvMenu.enabled = true;
	}
	
	public void HideMenu ()
	{
		cnvMenu.enabled = false;
	}
	
	public void ToggleMenu ()
	{
		if (cnvMenu.enabled)
			cnvMenu.enabled = false;
		else
			cnvMenu.enabled = true;
	}

	public void PlayDemo ()
	{
		Application.LoadLevel ("Demo");
	}

	public void Exit ()
	{
		Debug.Log ("Menu.Exit()");
		Application.Quit ();
		Debug.Log ("Application.Quit()");
	}

}