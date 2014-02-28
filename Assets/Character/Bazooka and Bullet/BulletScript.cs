using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	//How long the bullet will exist
	private float lifetime;
	
	void Start () {
		//Sets the time
		lifetime = Time.timeSinceLevelLoad;
		this.transform.GetComponent<ParticleSystem>().Play();
	}

	//Denotes the destruction of bullets when it hits
	//A monster or a scenery object
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.CompareTag("Scenery")) {
			Destroy(this.gameObject);
		}
		if (col.gameObject.CompareTag("Monster")) {
			col.transform.GetComponent<MonsterAnimator>().DealMonsterDamage(.5f);
			Destroy(this.gameObject);
		}
	}
	
	void Update () {
		//Destroys the object if it exists past a certain time.
		if (Time.timeSinceLevelLoad - lifetime > 2.5)
			Destroy(this.gameObject);
	}
}
