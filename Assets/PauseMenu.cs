using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	bool paused = false;
	
	void Update()
	{
		//Checks if escapse has been pressed
		if(Input.GetKeyDown(KeyCode.Escape))
			paused = togglePause();
	}

	//Creates the pause screen overlay
	//Checks if paused is true before overlaying
	void OnGUI()
	{
		if(paused)
		{
			GUILayout.Label("Game is paused!");
			if(GUILayout.Button("Return"))
				paused = togglePause();
		}
	}

	//Called when the escape key is pressed,
	//Stops time until the escape key is pressed again
	bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
			return(false);
		}
		else
		{
			Time.timeScale = 0f;
			return(true);    
		}
	}
}
