using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	private float lifetime;
	
	void Start () {
		lifetime = Time.timeSinceLevelLoad;
		this.transform.GetComponent<ParticleSystem>().Play();
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.CompareTag("Scenery")) {
			Destroy(this.gameObject);
		}
		if (col.gameObject.CompareTag("Monster")) {
			col.transform.GetComponent<MonsterAnimator>().dealDamage(.5f);
			Destroy(this.gameObject);
		}
	}
	
	void Update () {
		if (Time.timeSinceLevelLoad - lifetime > 2.5)
			Destroy(this.gameObject);
	}
}
