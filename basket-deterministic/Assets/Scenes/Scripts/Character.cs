using Gemserk.Lockstep;
using System.Collections.Generic;

public class Character : GameLogic {

	readonly Commands commands;

	readonly Movement movement;

	readonly List<Command> commandsList = new List<Command>();

	public Character (Commands commands, Movement movement)
	{
		this.commands = commands;
		this.movement = movement;
	}

	public void GameUpdate (float dt, int frame)
	{
		commands.GetCommands (frame, commandsList);

		for (int i = 0; i < commandsList.Count; i++) {
			var command = commandsList [i];

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
