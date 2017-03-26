using UnityEngine;
using Gemserk.Lockstep;
using System.Collections.Generic;

public class Character : MonoBehaviour, GameLogic {

	CommandsScript commandsScript;

	Movement movement;

	readonly List<Command> commands = new List<Command>();

	void Awake()
	{
		movement = GetComponent<Movement> ();
		commandsScript = FindObjectOfType<CommandsScript> ();
	}

	public void GameUpdate (float dt, int frame)
	{
		commandsScript.Commands.GetCommands (frame, commands);

		for (int i = 0; i < commands.Count; i++) {
			var command = commands [i];

			// TODO: check if moving this character...

			var commandMovement = command as CommandMovement;
			var commandMovementStop = command as CommandMovementStop;

			if (commandMovement != null) {
				// configure movement direction
				movement.Move(commandMovement.direction);
			} else if (commandMovementStop != null) {
				movement.Stop();
			}
		}
	}

}
