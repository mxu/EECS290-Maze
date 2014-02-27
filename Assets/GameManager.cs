using UnityEngine;
using System.Collections;

public static class GameManager {
	public delegate void GameEvent();
	public static event GameEvent MazeBuilt;

	//** The truth values are to denote if an item
	//** Already spawned in that square of the map
	//Number of monsters spawn
	private static int numberOfMonsters = -1;
	private static bool monsterSpawned;

	//Number of health packs to spawn
	private static int numberOfHealthPacks = -1;
	private static bool healthPackSpawned;

	//Number of batteries to be spawned
	private static int numberOfBatteryPacks = -1;
	private static bool batteryPackSpawn;


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


	//*******************************************//
	// I would like to consolidate this method  //
	// into one method for the monster, health  //
	// and batery spawning method in the future //
	//******************************************//


	/// <summary>
	/// Triggers the monster spawn.
	/// </summary>
	/// <param name="spawnCell">Spawn cell.</param>
	/// <param name="sizeOfGrid">Size of grid.</param>
	/// <param name="monsterPrefab">Monster prefab.</param>
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

	/// <summary>
	/// Triggers the health pack spawn.
	/// </summary>
	/// <param name="spawnCell">Spawn cell.</param>
	/// <param name="sizeOfGrid">Size of grid.</param>
	/// <param name="healthPackPrefab">Health pack prefab.</param>
	public static void TriggerHealthPackSpawn(Transform spawnCell, float sizeOfGrid, GameObject healthPackPrefab){
		if(monsterSpawned || batteryPackSpawn){
		}
		else{
			healthPackSpawned = false;
			Debug.Log("Entered health pack method");

			//Sets the number of health packs based on the level
			if(numberOfHealthPacks == -1){
				numberOfHealthPacks = ((int)sizeOfGrid/2) - 1;
				Debug.Log(numberOfHealthPacks);
			}

			//Spawns the health pack if the random number is in a certain range.
			if(Random.Range(0,10)%3 == 0){
				if(numberOfHealthPacks >0){
					GameObject currentPack = (GameObject)GameObject.Instantiate(healthPackPrefab, new Vector3(spawnCell.position.x, .65f, spawnCell.position.z), Quaternion.identity);
					currentPack.transform.localScale -= new Vector3(.05f, .05f, .05f);

					//Decrements health pack count
					numberOfHealthPacks--;
					Debug.Log("Health pack spawned!");
					healthPackSpawned = true;
				}
				else{
				}
			}
		}
	}

	/// <summary>
	/// Triggers the battery pack spawn.
	/// </summary>
	/// <param name="spawnCell">Spawn cell is the cell on the grid that this object will spawn on.</param>
	/// <param name="sizeOfGrid">Size of grid is the total size of the grid.</param>
	/// <param name="batteryPackPrefab">Battery pack prefab the prefab being passed in to be spawned.</param>
	public static void TriggerBatteryPackSpawn(Transform spawnCell, float sizeOfGrid, GameObject batteryPackPrefab){
		if(monsterSpawned || healthPackSpawned){
		}
		else{
			batteryPackSpawn = false;
			Debug.Log("Entered battery pack spawn method");

			//Sets the number of health packs based on the level
			if(numberOfBatteryPacks == -1){
				numberOfBatteryPacks = ((int)sizeOfGrid/2) + 1;
				Debug.Log(numberOfBatteryPacks);
			}

			//Spawn the battery packs
			if(numberOfBatteryPacks >0){
				GameObject currentBattery = (GameObject)GameObject.Instantiate(batteryPackPrefab, new Vector3(spawnCell.position.x, .65f, spawnCell.position.z), Quaternion.identity);
				currentBattery.transform.localScale -= new Vector3(.05f, .05f, .05f);

				//Decrements the health pack count
				numberOfBatteryPacks--;
				Debug.Log("The battery pack has spawned!");
				batteryPackSpawn = true;
			}
		}
	}


}
