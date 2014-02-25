using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public string buttonInfo;



	//Displayed at the start as the loading screen.
	void OnGUI () {
		if(GUI.Button(new Rect(30,30,150,30), buttonInfo)){
			GameManager.TriggerGameStart();
		}
		if(GUI.Button(new Rect(30,75,150,30), "Quit")){
			Application.Quit();
		}
	}

	//Called when the game is started
	private void GameStart () {

	}

	//Called on game over
	private void GameOver () {

	}
}
