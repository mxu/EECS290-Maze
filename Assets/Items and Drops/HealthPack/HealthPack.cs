using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	public HealthBar hp;
	public Vector3 upperBound;
	public Vector3 lowerBound;
	bool movingUp = true;


	
	void Start () {
		upperBound = new Vector3(transform.position.x, transform.position.y + .05f, transform.position.z);
		lowerBound = new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z);
	}

	void Update(){
		if(hp == null){
			hp = GameObject.FindGameObjectWithTag("Player").transform.GetComponentInChildren<HealthBar>();
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
