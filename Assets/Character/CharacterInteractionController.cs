using UnityEngine;
using System.Collections;

public class CharacterInteractionController : MonoBehaviour {

	public HealthBar hp;
	private float damage = .01f;

	void Start () {
		hp = transform.GetComponentInChildren<HealthBar>();
	}

	//Happens when a monsters attacks a player
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

	//Deals damage to the player
	 public void DealDamage(){
		if(hp.progress <= 0f){
			GameManager.TriggerGameOver();
		}
		else{
			hp.progress -= damage;
		}
	}
}
