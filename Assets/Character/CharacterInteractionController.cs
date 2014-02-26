using UnityEngine;
using System.Collections;

public class CharacterInteractionController : MonoBehaviour {

	public HealthBar hp;

	void Start () {
		hp = transform.GetComponentInChildren<HealthBar>();
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

}
