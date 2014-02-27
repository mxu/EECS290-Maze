using UnityEngine;
using System.Collections;

public class BatteryController : MonoBehaviour {

	public Light lightInstance;
	public Vector3 upperBound;
	public Vector3 lowerBound;
	bool movingUp = true;

	// Use this for initialization
	void Start () {
		upperBound = new Vector3(transform.position.x, transform.position.y + .05f, transform.position.z);
		lowerBound = new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z);
	}

	void Update(){
		if(lightInstance == null){
			lightInstance = GameObject.FindGameObjectWithTag("Player").transform.GetComponentInChildren<Light>();
		}
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
			                                       


	void OnTriggerEnter(Collider col){
		if(col.gameObject.CompareTag("Player")){
			if(lightInstance.intensity >= 1f){
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
		lightInstance.intensity += amount;
	}
}
