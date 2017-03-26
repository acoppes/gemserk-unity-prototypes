using UnityEngine;
using Gemserk.Lockstep;
using System.Collections.Generic;

public class GameLogicScript : MonoBehaviour, GameLogic
{
	readonly List<GameLogic> gameLogics = new List<GameLogic>();

	public CommandQueue commandsQueue;

	void Awake()
	{
		GetComponentsInChildren<GameLogic> (true, gameLogics);
		gameLogics.Remove (this);
	}

	#region GameLogic implementation

	public void GameUpdate (float dt, int frame)
	{
		if (commandsQueue.IsReady ())
			commandsQueue.SendCommands ();

		for (int i = 0; i < gameLogics.Count; i++) {
			var gameLogic = gameLogics [i];
			gameLogic.GameUpdate (dt, frame);
		}
	}

	#endregion
	
}
