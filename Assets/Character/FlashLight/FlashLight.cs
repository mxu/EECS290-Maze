using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	// Dims the flashlight over time
	// Will only dim as long as light is on
	void Update () {
		if(light.enabled == true){
			if(light.intensity >= .10f){
				light.intensity -= Time.deltaTime * .001f;
			}
			else{
			}
		}
		else{
		}
	}
}
