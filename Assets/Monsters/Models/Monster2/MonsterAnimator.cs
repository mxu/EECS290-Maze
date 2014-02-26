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
		float DistanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
		Vector3 LookDirection = Player.transform.position - transform.position;
		TimeToRun = (Physics.Raycast ((Vector3)transform.position,(Vector3) LookDirection, out hit)) && ((hit.transform.tag == "Player") && (DistanceToPlayer <= MinVisibleDistance));
		if(!TimeToRun){
			TimeToRun = ((Vector3.Angle(LookDirection, transform.forward)) < ViewAngle) && (Physics.Raycast (transform.position, LookDirection, out hit, MonsterSightRange)) && (hit.transform.tag == "Player");
		}
	}

	int[] GetMonsterCoordinates(){
		int Xpos = transform.position.x % 1 > .5 ? (int)transform.position.x + 1 : (int)transform.position.x;
		int Zpos = transform.position.z % 1 > .5 ? (int)transform.position.z + 1 : (int)transform.position.z;
		return new int[2]{Xpos, Zpos};
	}

		
	// Update is called once per frame
	void Update () {
		Player = GameObject.FindGameObjectWithTag("Player");
		CanSeePlayer();
		if ((Index == - 1 || (transform.position.x == Next.position.x && transform.position.z == Next.position.z)) && (!TimeToRun && !TimeToSlap)){
			int[] MonsterCoordinates = GetMonsterCoordinates();
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
