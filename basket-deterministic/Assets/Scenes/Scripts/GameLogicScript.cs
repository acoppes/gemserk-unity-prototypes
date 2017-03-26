using UnityEngine;
using Gemserk.Lockstep;
using System.Collections.Generic;

public class GameLogicScript : MonoBehaviour, GameLogic
{
	readonly List<GameLogic> gameLogics = new List<GameLogic>();

	CommandsScript commandsScript;

	void Awake()
	{
		commandsScript = GetComponent<CommandsScript> ();
		GetComponentsInChildren<GameLogic> (true, gameLogics);
		gameLogics.Remove (this);
	}

	#region GameLogic implementation

	public void GameUpdate (float dt, int frame)
	{
		for (int i = 0; i < gameLogics.Count; i++) {
			var gameLogic = gameLogics [i];
			gameLogic.GameUpdate (dt, frame);
		}

		commandsScript.Commands.RemoveCommands (frame);
	}

	#endregion
	
}
