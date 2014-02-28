using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	//HP bar being referenced
	public HealthBar hp;

	//Upper and lower bounds for moving the cube up and down in animating
	public Vector3 upperBound;
	public Vector3 lowerBound;

	//Doesn't allow the cube to move down if it is going up and vice versa
	bool movingUp = true;


	
	void Start () {
		//Sets the upper and lower bounds
		upperBound = new Vector3(transform.position.x, transform.position.y + .05f, transform.position.z);
		lowerBound = new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z);
	}

	void Update(){
		//Gets the HP bar being referenced
		if(hp == null){
			hp = GameObject.FindGameObjectWithTag("Player").transform.GetComponentInChildren<HealthBar>();
		}

		//Moves the cube up and down by comparing vectors
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

	//Called when the player walked into the block
	//If the player has full health, nothing will happen
	//Otherwise, the object is destroyed and health is increased.
	void OnTriggerEnter(Collider col){
		Debug.Log("Health collected!");
		if(col.gameObject.CompareTag("Player")){
			if(hp.progress >= 1f){
			}
			else{
				addHealth(.15f);
				Debug.Log("Health increased");
				Destroy(gameObject);
			}
		}

	}

	//Adds health to the character when picked up
	//Can only take values between 0.0 and 1.0
	void addHealth(float amount){
		hp.progress += amount;
	}
	
}
