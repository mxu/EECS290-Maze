﻿using UnityEngine;
using System.Collections;

public class CharacterInteractionController : MonoBehaviour {

	//Health bar
	public HealthBar hp;
	//Flashlight bar
	public LightBar lp;
	//Bullet
	public Transform bullet;
	//Reload Delay
	private float reloadDelay;
	//If the flash light is on
	private static bool flashLightOn = false;
	private bool reloaded;

	//Flashlight
	Light flashLight;

	/// <summary>
	/// Start this instance.
	/// Creates the HP and LightBar references
	/// Loads info from last level if needed, starts at 1
	/// Gets the flashlight instance
	/// </summary>
	void Start () {
		GameManager.LevelComplete += LevelComplete;
		hp = transform.GetComponentInChildren<HealthBar>();
		lp = transform.GetComponentInChildren<LightBar>();
		hp.progress = GameManager.loadHealth ();
		lp.progress = GameManager.loadLight ();
		flashLight = transform.GetComponentInChildren<Light>();
	}

	//Checks to see if the user presses F or ESC
	//F will turn the flashlight on or off
	//Esc will pause the game
	void Update(){
		checkFlashLight();
		
		lp.progress = flashLight.intensity;
		
		if ((Time.timeSinceLevelLoad - reloadDelay) > .7)
			reloaded = true;
		
		if (Input.GetButton("Fire1") && reloaded) {
			Debug.Log("BANG");
			shoot();
			reloaded = false;
			reloadDelay = Time.timeSinceLevelLoad;
		}
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

	/// <summary>
	/// Called when the level is over
	/// Saves the health bar between levels
	/// </summary>
	void LevelComplete ()
	{
		GameManager.saveHealth(hp.progress);
	}

	//Happens when a monsters attacks a player
	void OnControllerColliderHit(ControllerColliderHit hit) { 
		if(hit.gameObject.CompareTag("Monster")){
			hit.gameObject.GetComponent<MonsterAnimator>().animation.Play("bitchslap");
			DealPlayerDamage(.01f);
		} else if(hit.gameObject.CompareTag("Finish")) {
			Debug.Log("Finished");
			GameManager.TriggerLevelComplete();
		}
	}

	//Deals damage to the player, kills player if health gets too low
	 public void DealPlayerDamage(float damage){
		if(hp.progress <= 0f){
			GameManager.TriggerGameOver();
		}
		else{
			hp.progress -= damage;
		}
	}

	/// <summary>
	/// Called when the gun is fired
	/// Defines the bullet object with its direction and force
	/// Animates gun
	/// </summary>
	void shoot () {
		Transform shot = (Transform)Instantiate(bullet, bullet.position, bullet.rotation);
		shot.transform.localScale = new Vector3(.03f,.03f,.03f);
		shot.gameObject.AddComponent("BulletScript");
		shot.rigidbody.AddForce(new Vector3((bullet.position.x + bullet.rigidbody.velocity.x - this.transform.position.x) * 800, (bullet.position.y + bullet.rigidbody.velocity.y - this.transform.position.y ) * 800,(bullet.position.z + bullet.rigidbody.velocity.z - this.transform.position.z) * 800));
		this.transform.GetChild(3).GetChild(0).animation.Play("shoot");
		this.transform.GetChild(3).GetChild(0).audio.Play ();
		
	}
	
	void OnTriggerStay(Collider c){
		if(c.gameObject.tag.Equals("Monster")){
			c.gameObject.GetComponent<MonsterAnimator>().Beatdown();
		}
	}
	
	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag.Equals("Monster")){
			c.gameObject.GetComponent<MonsterAnimator>().LookAtPlayer();
		}
	}
	
	void OnTriggerExit(Collider c){
		if(c.gameObject.tag.Equals("Monster")){
			c.gameObject.GetComponent<MonsterAnimator>().SetSlap(false);
		}
	}

}
