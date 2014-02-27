using UnityEngine;
using System.Collections;

public class CharacterInteractionController : MonoBehaviour {

	public HealthBar hp;
	public GameObject MonsterRagdoll;
	private float damage = .01f;

	void Start () {
		hp = transform.GetComponentInChildren<HealthBar>();
		}
	
	void OnTriggerStay(Collider c){
		if(c.gameObject.tag.Equals("Monster")){
			c.gameObject.GetComponent<MonsterAnimator>().Beatdown();
			if(Input.GetKeyDown(KeyCode.K)){
				KillMonsters(c.gameObject);
			}
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

	void KillMonsters(GameObject Monster){
		Transform T = Monster.transform;
		Destroy(Monster);
		GameObject Ragdoll = (GameObject)GameObject.Instantiate(MonsterRagdoll, new Vector3(T.position.x, T.position.y, T.position.z), T.rotation); 
		//Ragdoll.transform.Rotate(new Vector3(-20f, 0f, 0f));
		//Ragdoll.rigidbody.AddForce (Vector3.back * 100);
		Destroy(Ragdoll, 10f);
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
