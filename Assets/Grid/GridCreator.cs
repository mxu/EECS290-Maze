using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

/**
 * Creates a grid of specified dimensions and generates a procedural maze using a
 * modified form of Prim's Algorithm.
 * @author Timothy Sesler
 * @author tds45
 * @date 4 February 2014
 * 
 * Adapted from work provided online by Austin Takechi 
 * Contact: MinoruTono@Gmail.com
 */ 
public class GridCreator : MonoBehaviour {
	
	public Transform CellPrefab;
	public Vector3 Size;
	public Transform[,] Grid;
	public GameObject player;

	//Monsters
	public GameObject monsters;

	public int NumMonsters;
	public GameObject MonsterPrefab;

	
	// Use this for initialization
	void Start () {
		GameManager.MazeBuilt += MazeBuilt;
		CreateGrid();
		SetRandomNumbers();
		SetAdjacents();
		SetStart();
		FindNext();
	}

	void MazeBuilt ()
	{
		BuildWalls();
		SpawnPlayer();
		SpawnMonsters();
	}

	private void SpawnMonsters() {
		// Spawn monsters inside the maze once it's built
		// Keep track of which cells have already spawned a monster
		List<Transform> occupied = new List<Transform>();
		// # of monsters = 10% of number of non-wall cells
		int numMonsters = Mathf.RoundToInt(PathCells.Count * 0.1f);
		Debug.Log("Spawning " + numMonsters + " monsters along " + PathCells.Count + "  cells");
		while(numMonsters-- > 0) {
			// Get an empty cell
			Transform cell;
			do {
				cell = PathCells[Random.Range(1, PathCells.Count - 2)];
			} while(occupied.Contains(cell));
			// Put a monster on it
			occupied.Add(cell);
			GameObject monster = (GameObject)Instantiate(monsters, new Vector3(cell.position.x, 0.5f, cell.position.z), Quaternion.identity);
			monster.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			Debug.Log("Monster #" + numMonsters + " spawned on cell " + cell.name);
		}
		
	}

	//To be called when start is pressed.
	public static void Build(){
	}


	// expose function for checking if a cell is part of the path
	public bool isOpen(int x, int z) {
		return x >= 0 && x < Size.x && z >= 0 && z < Size.z && PathCells.Contains (Grid [x, z]);
	}

	// Creates the grid by instantiating provided cell prefabs.
	void CreateGrid () {
		Size.x = Size.z = GameManager.getLevel() * 3 + 2;
		Debug.Log ("Level " + GameManager.getLevel() + ": " + Size.x + "x" + Size.z);
		Grid = new Transform[(int)Size.x,(int)Size.z];
		
		// Places the cells and names them according to their coordinates in the grid.
		for (int x = 0; x < Size.x; x++) {
			for (int z = 0; z < Size.z; z++) {
				Transform newCell;
				newCell = (Transform)Instantiate(CellPrefab, new Vector3(x, 0, z), Quaternion.identity);
				newCell.name = string.Format("({0},0,{1})", x, z);
				newCell.parent = transform;
				newCell.GetComponent<CellScript>().Position = new Vector3(x, 0, z);
				Grid[x,z] = newCell;
			}
		}
		// Centers the camera on the maze.
		// Feel free to adjust this as needed.
		//Camera.main.transform.position = Grid[(int)(Size.x / 2f),(int)(Size.z / 2f)].position + Vector3.up * 20f;
		//Camera.main.orthographicSize = Mathf.Max(Size.x * 0.55f, Size.z * 0.5f);
	}
	
	// Sets a random weight to each cell.
	void SetRandomNumbers () {
		foreach (Transform child in transform) {
			int weight = Random.Range(0,10);
			child.GetComponentInChildren<TextMesh>().text = weight.ToString();
			child.GetComponent<CellScript>().Weight = weight;
		}
	}
	
	// Determines the adjacent cells of each cell in the grid.
	void SetAdjacents () {
		for(int x = 0; x < Size.x; x++){
			for (int z = 0; z < Size.z; z++) {
				Transform cell;
				cell = Grid[x,z];
				CellScript cScript = cell.GetComponent<CellScript>();
				
				if (x - 1 >= 0) {
					cScript.Adjacents.Add(Grid[x - 1, z]);
				}
				if (x + 1 < Size.x) {
					cScript.Adjacents.Add(Grid[x + 1, z]);
				}
				if (z - 1 >= 0) {
					cScript.Adjacents.Add(Grid[x, z - 1]);
				}
				if (z + 1 < Size.z) {
					cScript.Adjacents.Add(Grid[x, z + 1]);
				}
				
				cScript.Adjacents.Sort(SortByLowestWeight);
			}
		}
	}
	
	// Sorts the weights of adjacent cells.
	// Check the link for more info on custom comparators and sorting.
	// http://msdn.microsoft.com/en-us/library/0e743hdt.aspx
	int SortByLowestWeight (Transform inputA, Transform inputB) {
		int a = inputA.GetComponent<CellScript>().Weight;
		int b = inputB.GetComponent<CellScript>().Weight;
		return a.CompareTo(b);
	}
	
	/*********************************************************************
	 * Everything after this point pertains to generating the actual maze.
	 * Look at the Wikipedia page for more info on Prim's Algorithm.
	 * http://en.wikipedia.org/wiki/Prim%27s_algorithm
	 ********************************************************************/ 
	public List<Transform> PathCells;			// The cells in the path through the grid.
	public List<List<Transform>> AdjSet;		// A list of lists representing available adjacent cells.
	/** Here is the structure:
	 *  AdjSet{
	 * 		[ 0 ] is a list of all the cells
	 *      that have a weight of 0, and are
	 *      adjacent to the cells in the path
	 *      [ 1 ] is a list of all the cells
	 *      that have a weight of 1, and are
	 * 		adjacent to the cells in the path
	 *      ...
	 *      [ 9 ] is a list of all the cells
	 *      that have a weight of 9, and are
	 *      adjacent to the cells in the path
	 * 	}
	 *
	 * Note: Multiple entries of the same cell
	 * will not appear as duplicates.
	 * (Some adjacent cells will be next to
	 * two or three or four other path cells).
	 * They are only recorded in the AdjSet once.
	 */  
	
	// Initializes the sets and the starting cell.
	void SetStart () {
		PathCells = new List<Transform>();
		AdjSet = new List<List<Transform>>();
		
		for (int i = 0; i < 10; i++) {
			AdjSet.Add(new List<Transform>());	
		}
		
		Grid[0, 0].renderer.material.color = Color.green;
		AddToSet(Grid[0, 0]);
	}
	
	// Adds a cell to the set of visited cells.
	void AddToSet (Transform cellToAdd) {
		PathCells.Add(cellToAdd);
		
		foreach (Transform adj in cellToAdd.GetComponent<CellScript>().Adjacents) {
			adj.GetComponent<CellScript>().AdjacentsOpened++;
			
			if (!PathCells.Contains(adj) && !(AdjSet[adj.GetComponent<CellScript>().Weight].Contains(adj))) {
				AdjSet[adj.GetComponent<CellScript>().Weight].Add(adj);
			}
		}
	}
	
	// Determines the next cell to be visited.
	void FindNext () {
		Transform next;
		
		do {
			bool isEmpty = true;
			int lowestList = 0;
			
			// We loop through each sub-list in the AdjSet list of lists, until we find one with a count of more than 0.
			// If there are more than 0 items in the sub-list, it is not empty.
			// We've found the lowest sub-list, so there is no need to continue searching.
			for (int i = 0; i < 10; i++) {
				lowestList = i;
				
				if (AdjSet[i].Count > 0) {
					isEmpty = false;
					break;
				}
			}
			
			// The maze is complete.
			if (isEmpty) { 
				Debug.Log("Generation completed in " + Time.timeSinceLevelLoad + " seconds."); 
				//CancelInvoke("FindNext"); ???
				Transform finish = PathCells[PathCells.Count - 1];
				finish.renderer.material.color = Color.red;
				finish.tag = "Finish";
				foreach (Transform cell in Grid) {
					// Removes displayed weight
					cell.GetComponentInChildren<TextMesh>().renderer.enabled = false;
					
					if (!PathCells.Contains(cell)) {
						// HINT: Try something here to make the maze 3D
						cell.renderer.material.color = new Color(.9f,.9f,.9f);
						cell.transform.localScale = new Vector3(1, 2, 1);
						cell.transform.Translate(new Vector3(0, 0.5f, 0));
					}
				}

				GameManager.TriggerMazeBuilt();
				return;
			}
			// If we did not finish, then:
			// 1. Use the smallest sub-list in AdjSet as found earlier with the lowestList variable.
			// 2. With that smallest sub-list, take the first element in that list, and use it as the 'next'.
			next = AdjSet[lowestList][0];
			// Since we do not want the same cell in both AdjSet and Set, remove this 'next' variable from AdjSet.
			AdjSet[lowestList].Remove(next);
		} while (next.GetComponent<CellScript>().AdjacentsOpened >= 2);	// This keeps the walls in the grid, otherwise Prim's Algorithm would just visit every cell
		
		// The 'next' transform's material color becomes white.
		next.renderer.material.color = Color.white;
		// We add this 'next' transform to the Set our function.
		AddToSet(next);
		// Recursively call this function as soon as it finishes.
		Invoke("FindNext", 0);
		// FindNext();
	}
	
	void BuildWalls() {
		Vector3[] pos = {
			new Vector3(Size.x / 2 - 0.5f, 0.5f, -1),
			new Vector3(Size.x / 2 - 0.5f, 0.5f, Size.z),
			new Vector3(-1, 0.5f, Size.z / 2 - 0.5f),
			new Vector3(Size.x, 0.5f, Size.z / 2 - 0.5f)
		};
		Vector3[] scales = {
			new Vector3 (Size.x, 2, 1),
			new Vector3 (Size.x, 2, 1),
			new Vector3 (1, 2, Size.z),
			new Vector3 (1, 2, Size.z)
		};
		Debug.Log("Building " + Size.x + "x" + Size.z + " wall");
		for(int i = 0; i < 4; i++) {
			Transform wall = (Transform)Instantiate (CellPrefab, pos[i], Quaternion.identity);
			wall.renderer.material.color = Color.black;
			wall.localScale = scales[i];
			wall.GetComponentInChildren<TextMesh> ().renderer.enabled = false;
		}
	}

	//Remove this method later, make it in the game event manager in the long run.
	void SpawnPlayer(){
		Instantiate (player, new Vector3 (0, 1, 0), Quaternion.identity);
		Debug.Log ("The player has spawned!");
	}

	// Called once per frame.
	void Update() {
		
		// Pressing 'F1' will generate a new maze.
		if (Input.GetKeyDown(KeyCode.F1)) {
			GameManager.TriggerLevelComplete();
			//Application.LoadLevel(0);
		}
	}
}
