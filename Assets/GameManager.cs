using UnityEngine;
using System.Collections;

public static class GameManager {
	public delegate void GameEvent();
	public static event GameEvent MazeBuilt, LevelComplete;

	//Level of the game
	private static int level = 1;
	//Variable used to save and restore the player's health between levels
	private static float playerHealth = 1f;
	//Stores the light level between levels
	private static float lightLevel = 1f;

	//Called when the maze is done being built
	public static void TriggerMazeBuilt() {
		if(MazeBuilt != null) MazeBuilt();
	}

	//Called when the user completes the level
	//Increments the level
	//Sets the game event for Level complete if it isn't
	public static void TriggerLevelComplete() {
		level++;
		if(LevelComplete != null) LevelComplete();
		TriggerGameStart();
	}

	public static int getLevel() {
		return level;
	}

	//Saves the health left on the player between levels
	//@param h the health to be saved
	public static void saveHealth(float h) {
		playerHealth = h;
	}

	public static float loadHealth() {
		return playerHealth;
	}

	//Saves the light amount left in the flashlight
	//@param l the light to be saved
	public static void saveLight(float l) {
		lightLevel = l;
	}
	
	public static float loadLight() {
		return lightLevel;
	}

	//Starts the game
	public static void TriggerGameStart(){
		MazeBuilt = null;
		LevelComplete = null;
		Application.LoadLevel("Maze");
	}

	//Called when the player has zero health.
	public static void TriggerGameOver(){
		Application.LoadLevel("GameOverMenue");
	}

	// Removed TriggerMonsterSpawn and moved the logic into the GridCreator's MazeBuilt handler -Mike
}
