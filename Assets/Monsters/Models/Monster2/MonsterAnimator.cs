using UnityEngine;
using System.Collections;

public class MonsterAnimator : MonoBehaviour {
	
	public float health = 1.0f;
//	private float runSpeed = .02f;
//	private float walkSpeed = .01f;
//	private float turnSpeed = .005f;
//	private float timer = 3f;

	// Use this for initialization
//	void Start () {
//		transform.localScale -= new Vector3(.8f, .8f, .8f);
//	}

//	void OnCollisionEnter(Collision col){
//		Debug.Log("Get slapped");
//		Debug.Log(col.collider.tag);
//		if(col.collider.CompareTag("Player")){
//			animation.Play("bitchslap");
//		}
//	}

	// Update is called once per frame
	void Update () {
	
		if (this.health <= 0) {
			Destroy(this.gameObject);
		}
//		GameObject FPS = GameObject.FindGameObjectWithTag("Player");
//
//			if (((FPS.transform.position.x - transform.position.x) <= 10) && ((FPS.transform.position.x - transform.position.x) >=-10)  && ((FPS.transform.position.z - transform.position.z) <=10) && ((FPS.transform.position.z - transform.position.z) >= -10)){
//				animation.Play ("run");
//				transform.position = Vector3.Lerp(transform.position, (new Vector3(FPS.transform.position.x, .5f, FPS.transform.position.z)), runSpeed);
//				Vector3 targetPostition = new Vector3(FPS.transform.position.x, transform.position.y, FPS.transform.position.z);
//				transform.LookAt(targetPostition);
//			}
//			else {
//				animation.Play("walk");
//				transform.Translate (Vector3.forward * walkSpeed);
//				timer -= Time.deltaTime;
//				int turnDegrees = Random.Range (0, 90);
//				int turnDirection = Random.Range (0, 1);
//				if (turnDirection == 1 && timer <0 ){
//					transform.Rotate(Vector3.left * turnSpeed, turnDegrees);
//					timer = 15;
//				}
//				else if (timer <= 0){
//					transform.Rotate(Vector3.right * turnSpeed, turnDegrees);
//					timer = 15;
//				}
//			}


	}
	
	public void dealDamage(float dmg) {
		this.health -= dmg;
	}

}
