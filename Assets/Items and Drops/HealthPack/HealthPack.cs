using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	public HealthBar hp;
	
	void Start () {
		hp = GameObject.FindGameObjectWithTag("Player").transform.GetComponentInChildren<HealthBar>();
	}

	//Called when the player walked into the block
	//If the player has full health, nothing will happen
	//Otherwise, the object is destroyed and health is increased.
	void OnTriggerEnter(Collider col){
		Debug.Log("Health collected!");
		if(col.gameObject.CompareTag("Player")){
			if(hp.progress == 1f){
			}
			else{
				addHealth(.15f);
				Debug.Log("Health increased");
				DestroyObject(this);
			}
		}

	}

	//Adds health to the character when picked up
	//Can only take values between 0.0 and 1.0
	void addHealth(float amount){
		hp.progress += amount;
	}
	
}
