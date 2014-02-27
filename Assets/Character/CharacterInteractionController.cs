using UnityEngine;
using System.Collections;

public class CharacterInteractionController : MonoBehaviour {

	public HealthBar hp;
	public LightBar lp;
	public GameObject flight;
	public Transform bullet;
	public float reloadDelay;

	private bool reloaded = true;

	void Start () {
		hp = transform.GetComponentInChildren<HealthBar>();
		lp = transform.GetComponentInChildren<LightBar>();
		flight = transform.GetChild(3).GetChild(1).gameObject;
	}

	//Happens when a monsters attacks a player
	void OnControllerColliderHit(ControllerColliderHit hit){
		if(hit.gameObject.CompareTag("Monster")){
			hit.gameObject.GetComponent<MonsterAnimator>().animation.Play("bitchslap");

			if(hp.progress <= 0f){
				GameManager.TriggerGameOver();
			}
			else{
				dealDamage(.010f);
			}
		}
	}

	//Deals damage to the player
	 void dealDamage(float damage){
		hp.progress -= damage;
	}
	
	void Update () {
		flight.GetComponent<FlashLightScript>().degradeLight();
		lp.progress = flight.GetComponent<FlashLightScript>().getProgress();
		
		if ((Time.timeSinceLevelLoad - reloadDelay) > .7)
			reloaded = true;
			
		if (Input.GetButton("Fire1") && reloaded) {
			Debug.Log("BANG");
			shoot();
			reloaded = false;
			reloadDelay = Time.timeSinceLevelLoad;
		}
	}
	
	void shoot () {
		Transform shot = (Transform)Instantiate(bullet, bullet.position, bullet.rotation);
		shot.transform.localScale = new Vector3(.03f,.03f,.03f);
		shot.gameObject.AddComponent("BulletScript");
		shot.rigidbody.AddForce(new Vector3((bullet.position.x + bullet.rigidbody.velocity.x - this.transform.position.x) * 800, (bullet.position.y + bullet.rigidbody.velocity.y - this.transform.position.y ) * 800,(bullet.position.z + bullet.rigidbody.velocity.z - this.transform.position.z) * 800));
		this.transform.GetChild(3).GetChild(0).animation.Play("shoot");
		this.transform.GetChild(3).GetChild(0).audio.Play ();
		
	}
}
