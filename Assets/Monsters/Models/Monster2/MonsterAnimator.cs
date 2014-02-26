using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterAnimator : MonoBehaviour {
	

	private float RunSpeed = .5f;
	private float WalkSpeed = .15f;
	private List<Transform> Path;
	private Transform Next;
	private Transform Previous;
	private int Index;
	private Vector3 Target;
	private bool TimeToSlap = false;
	private bool TimeToRun = false;
	private GameObject Player = null;
	private float ViewAngle = 70;
	private float MinVisibleDistance = .75f;
	private float MonsterSightRange = 2f;


	// Use this for initialization
	void Start () {
		Index = - 1;
		Path = GameObject.Find("Grid").GetComponent<GridCreator>().PathCells;
		Previous = transform;
		Player = new GameObject();
		Debug.Log (Player);
	}

	//Turns this monster towards the player GameObject
	public void LookAtPlayer(){
		transform.LookAt(new Vector3(Player.transform.position.x, .5f, Player.transform.position.z));
	}

	//Used for Setting Slap bool outside of this script
	public void SetSlap(bool slap){
		TimeToSlap = slap;
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag.Equals ("Player")){
			LookAtPlayer();
			TimeToSlap = true;
		}
	}

	//deal damage, play slapping animation
	public void Beatdown(){
		LookAtPlayer();
		animation.Play ("bitchslap");
		TimeToSlap = true;
		Index = -1;
		Player.GetComponent<CharacterInteractionController>().DealDamage();
	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.tag.Equals ("Player")){
			Beatdown();
		}
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag.Equals ("Player")){
			TimeToSlap = false;
		}
	}

	void CanSeePlayer(){
		RaycastHit hit;
		Player = GameObject.FindGameObjectWithTag("Player");

		float DistanceToPlayer = Vector3.Distance(/*Player.*/transform.position, transform.position);
		Vector3 LookDirection = Player.transform.position - transform.position;
		if(Physics.Raycast ((Vector3)transform.position,(Vector3) LookDirection, out hit)){
			if((hit.transform.tag == "Player") && (DistanceToPlayer <= MinVisibleDistance)){
				TimeToRun = true;
			}
			else{
				TimeToRun = false;
			}
		}
		if((Vector3.Angle(LookDirection, transform.forward)) < ViewAngle){ // Detect if player is within the field of view
			if (Physics.Raycast (transform.position, LookDirection, out hit, MonsterSightRange)) {
				if (hit.transform.tag == "Player") {
					//Debug.Log("Can see player");
					TimeToRun = true;
				}else{
					//Debug.Log("Can not see player");
					TimeToRun = false;
				}
			}
		}
	}

	int[] GetCoordinates(){
		int Xpos;
		int Zpos;
		if (transform.position.x < 0){
			Xpos = 0;
		}
		else if ((transform.position.x % 1) > .5){
			Xpos = (int)transform.position.x + 1;
		}
		else{
			Xpos = (int)transform.position.x;
		}
		if (transform.position.z < 0){
			Zpos = 0;
		}
		else if (transform.position.z % 1 > .5){
			Zpos = (int)transform.position.z + 1;
		}
		else{
			Zpos = (int)transform.position.z;
		}
		return  new int[2]{Xpos, Zpos};
	}

		
	// Update is called once per frame
	void Update () {
		Player = GameObject.FindGameObjectWithTag("Player");
		CanSeePlayer();
		if ((Index == - 1 || (transform.position.x == Next.position.x && transform.position.z == Next.position.z)) && (!TimeToRun && !TimeToSlap)){

			int[] MonsterCoordinates = GetCoordinates();
			List<Transform> CurrentCellOpen = new List<Transform>();
			GameObject CurrentCell = GameObject.Find("(" + MonsterCoordinates[0] + "," + "0" + "," + MonsterCoordinates[1] + ")");
			List<Transform> CurrentCellAdj = CurrentCell.GetComponent<CellScript>().Adjacents;
			foreach(Transform t in CurrentCellAdj){
				foreach(Transform OpenCell in Path){
					if (t.Equals(OpenCell)){
						CurrentCellOpen.Add(t);
					}
				}
			}
			Index = Random.Range (0, CurrentCellOpen.Count - 1);
			if (CurrentCellOpen[Index].position.Equals(Previous.position) && CurrentCellOpen.Count > 1 && Index < CurrentCellOpen.Count - 1){
				Index++;
			}
			else if(CurrentCellOpen[Index].position.Equals(Previous.position) && CurrentCellOpen.Count > 1 && Index > 0){
				Index--;
			}
			if(!Next.Equals (null)){
				Previous = Next;
			}
			Next = CurrentCellOpen[Index];
			Target = new Vector3(Next.position.x, transform.position.y, Next.position.z);
			transform.LookAt (Target);
			animation.Play("walk");
		}
		if(!TimeToRun && !TimeToSlap){
			transform.position = Vector3.MoveTowards(transform.position, Target, WalkSpeed * Time.deltaTime);
		}
		else if(TimeToRun && !TimeToSlap){
			Index = - 1;
			Target = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
			animation.Play ("run");
			LookAtPlayer();
			transform.position = Vector3.MoveTowards(transform.position, Target, RunSpeed * Time.deltaTime);
		}
	}
}
