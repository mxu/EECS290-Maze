using UnityEngine;
using System.Collections;

public static class GameManager {
	public delegate void GameEvent();
	public static event GameEvent MazeBuilt, LevelComplete;

	private static int level = 1;
	//Variable used to save and restore the player's health between levels
	private static float playerHealth = 1f;
	
	public static void TriggerMazeBuilt() {
		if(MazeBuilt != null) MazeBuilt();
	}

	public static void TriggerLevelComplete() {
		level++;
		if(LevelComplete != null) LevelComplete();
		TriggerGameStart();
	}

	public static int getLevel() {
		return level;
	}

	public static void saveHealth(float h) {
		playerHealth = h;
	}

	public static float loadHealth() {
		return playerHealth;
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
