using UnityEngine;
using System.Collections;

public static class GameManager {
	public delegate void GameEvent();
	public static event GameEvent MazeBuilt;

	//Number of monsters spawn
	private static int numberOfMonsters = -1;
	private static bool monsterSpawned;

	//Number of health packs to spawn
	private static int numberOfHealthPacks = -1;


	//Called when the maze is completed.
	public static void TriggerMazeBuilt() {
		if(MazeBuilt != null) MazeBuilt();
	}

	//Starts the game
	public static void TriggerGameStart(){
		Application.LoadLevel("Maze");
	}

	//Called when the player has zero health.
	public static void TriggerGameOver(){
		Application.LoadLevel("GameOverMenue");

		//Reset number of monsters for next game.
		numberOfMonsters = -1;
	}

	//Spawn Monsters on map
	public static void TriggerMonsterSpawn(Transform spawnCell, float sizeOfGrid, GameObject monsterPrefab){
		monsterSpawned = false;
		if(numberOfMonsters == -1){
			numberOfMonsters = (int)sizeOfGrid/2;
			Debug.Log(numberOfMonsters);
		}
		if(Random.Range(0, 10)%2 == 0){
			if(numberOfMonsters > 0){
				GameObject currentMonster = (GameObject)GameObject.Instantiate(monsterPrefab, new Vector3(spawnCell.position.x, 0.5f, spawnCell.position.z), Quaternion.identity);
				currentMonster.transform.localScale -= new Vector3(.8f, .8f, .8f);
				numberOfMonsters--;
				Debug.Log("Monster spawned");
				monsterSpawned = true;
			}
			else{
			}
		}

	}

	//Method used to generate healthpacks on map
	public static void TriggerHealthPackSpawn(Transform spawnCell, float sizeOfGrid, GameObject healthPackPrefab){
		if(monsterSpawned){
		}
		else{
			Debug.Log("Entered health pack method");
			if(numberOfHealthPacks == -1){
				numberOfHealthPacks = ((int)sizeOfGrid/2) - 1;
				Debug.Log(numberOfHealthPacks);
			}
			if(Random.Range(0,10)%3 == 0){
				if(numberOfHealthPacks >0){
					GameObject currentPack = (GameObject)GameObject.Instantiate(healthPackPrefab, new Vector3(spawnCell.position.x, .75f, spawnCell.position.z), Quaternion.identity);
					currentPack.transform.localScale -= new Vector3(.05f, .05f, .05f);
					numberOfHealthPacks--;
					Debug.Log("Health pack spawned!");
				}
				else{
				}
			}
		}
	}


}
