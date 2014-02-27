using UnityEngine;
using System.Collections;

public class CharacterInteractionController : MonoBehaviour {

	public HealthBar hp;

	private static bool flashLightOn = false;
	Light flashLight;

	void Start () {
		hp = transform.GetComponentInChildren<HealthBar>();
		flashLight = transform.GetComponentInChildren<Light>();
	}

	//Happens when a monsters attacks a player
	void OnControllerColliderHit(ControllerColliderHit hit){
		Debug.Log("Collide!");
		if(hit.gameObject.CompareTag("Monster")){
			hit.gameObject.GetComponent<MonsterAnimator>().animation.Play("bitchslap");

			if(hp.progress <= 0f){
				GameManager.TriggerGameOver();
			}
			else{
				dealDamage(.010f);
				Debug.Log ("The player has been hit!");
			}
		}
	}

	//Deals damage to the player
	//Can only take values between 0.0 and 1.0
	 void dealDamage(float damage){
		hp.progress -= damage;
	}

	//Checks to see if the user presses F or ESC
	//F will turn the flashlight on or off
	//Esc will pause the game
	void Update(){
		checkFlashLight();

	}

	//Checks if the F key is down and whether the boolean is on or off.
	void checkFlashLight(){
		if(Input.GetKeyDown(KeyCode.F) && flashLightOn == false){
			
			flashLightOn = true; //If the f key is down and the boolean is false, it sets the boolean to true.
			flashLight.enabled = !flashLight.enabled;
			
		} 
		else {
			
			if(Input.GetKeyDown(KeyCode.F) && flashLightOn == true) {
				
				flashLightOn = false;//If the f key is down and the boolean is true, it sets the boolean to false.
				flashLight.enabled = !flashLight.enabled;
			}
			
		}
	}

}
