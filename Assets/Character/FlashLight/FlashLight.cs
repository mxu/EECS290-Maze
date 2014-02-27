using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		light.intensity = GameManager.loadLight();
		GameManager.LevelComplete += LevelComplete;
	}

	void LevelComplete ()
	{
		GameManager.saveLight(light.intensity);
	}

	// Update is called once per frame
	// Dims the flashlight over time
	// Will only dim as long as light is on
	void Update () {
		if(light.enabled == true && light.intensity >= .10f)
				light.intensity -= Time.deltaTime * (1f/60f);			
	} 
}
