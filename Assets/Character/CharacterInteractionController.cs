using UnityEngine;
using System.Collections;

public class CharacterInteractionController : MonoBehaviour {

	public HealthBar hp;

	void Start () {
		GameManager.LevelComplete += LevelComplete;
		hp = transform.GetComponentInChildren<HealthBar>();
		hp.progress = GameManager.loadHealth ();
	}

	void LevelComplete ()
	{
		GameManager.saveHealth(hp.progress);
	}

	//Happens when a monsters attacks a player
	void OnControllerColliderHit(ControllerColliderHit hit) { 
		if(hit.gameObject.CompareTag("Monster")){
			hit.gameObject.GetComponent<MonsterAnimator>().animation.Play("bitchslap");

			if(hp.progress <= 0f){
				GameManager.TriggerGameOver();
			}
			else{
				dealDamage(.010f);
				Debug.Log ("The player has been hit!");
			}
		} else if(hit.gameObject.CompareTag("Finish")) {
			Debug.Log("Finished");
			GameManager.TriggerLevelComplete();
		}
	}

	//Deals damage to the player
	 void dealDamage(float damage){
		hp.progress -= damage;
	}

}
