using UnityEngine;
using System.Collections;

public class BatteryController : MonoBehaviour {

	//Instance of the light being referenced
	public GameObject lightInstance;

	//How high the item will float up
	public Vector3 upperBound;
	//How low the item will float down
	public Vector3 lowerBound;

	//Called if the item is moving up so as not to
	//Let it move down
	bool movingUp = true;

	// Use this for initialization
	void Start () {
		//Sets the upper and lower bounds
		upperBound = new Vector3(transform.position.x, transform.position.y + .05f, transform.position.z);
		lowerBound = new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z);
	}

	void Update(){
		//Gets instance of light and sets it to be referenced later
		if(lightInstance == null){
			lightInstance = GameObject.FindGameObjectWithTag("Player");
		}

		//Called to animate the cube and move it up or down by comparing vectors
		if(movingUp){
			transform.position = Vector3.Lerp(transform.position, upperBound, Time.deltaTime);
			if(Vector3.Magnitude(upperBound - transform.position) < .01f){
				movingUp = !movingUp;
			}
		}
		else{
			transform.position = Vector3.Lerp(transform.position, lowerBound, Time.deltaTime);
			if(Vector3.Magnitude(lowerBound - transform.position) < .01f){
				movingUp = !movingUp;
			}
		}


	}
			                                       

	//When the player runs into the item
	//Checks to see that the light intensity is 1
	//Otherwise, it adds to the intsenity and ups the flashlight power
	void OnTriggerEnter(Collider col){
		if(col.gameObject.CompareTag("Player")){
			if(lightInstance.transform.GetChild(3).GetChild(1).GetComponent<Light>().intensity >= 1f){
			}
			else{
				addElectricity(.1f);
				Debug.Log("Battery increased");
				Destroy(gameObject);
			}
		}

	}

	//Adds the amount to the intensity of the light
	void addElectricity(float amount){
		lightInstance.transform.GetChild(3).GetChild(1).GetComponent<Light>().intensity += amount;
	}
}
