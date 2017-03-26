using System.Collections.Generic;
using UnityEngine;
using Gemserk.Lockstep;

public class CommandMovement : CommandBase
{
	public Vector2 direction;

	public override string ToString ()
	{
		return string.Format ("[CommandMovement:{0}]", direction);
	}
}

public class CommandMovementStop : CommandBase
{
	public override string ToString ()
	{
		return string.Format ("[CommandMovementStop]");
	}
}

public class LockstepEngine : MonoBehaviour, CommandProcessor, CommandSender {

	LockstepFixedUpdate lockstep;
	Commands commandsList;

	GameLogicScript gameLogic;

	void Awake()
	{
		CommandProcessor commandProcesor = new CommandProcessorList (new List<CommandProcessor>() {
			this
		});

		commandsList = new CommandsList ();

		LockstepLogic lockstepLogic = new CommandsListLockstepLogic (commandsList, commandProcesor);
		lockstep = new LockstepFixedUpdate (lockstepLogic);

		lockstep.GameFramesPerLockstep = 1;
		lockstep.FixedStepTime = 16.0f / 1000.0f;

		gameLogic = GetComponent<GameLogicScript> ();

		lockstep.SetGameLogic (gameLogic);

		lockstep.Init ();

		// to start executing...
		commandsList.AddCommand (new CommandBase () { 
			ProcessFrame = lockstep.GetFirstLockstepFrame()
		});

		gameLogic.commandsQueue = new CommandQueueBase (lockstep, this);
	}

//	void FixedUpdate()
//	{
//		if (commandsQueue.IsReady ())
//			commandsQueue.SendCommands ();
//	}

	#region CommandSender implementation

	public void SendEmpty ()
	{
		this.commandsList.AddCommand (new CommandBase () { 
			ProcessFrame = lockstep.GetNextLockstepFrame()
		});
	}

	public void SendCommands (List<Command> commands)
	{
		foreach (var command in commands) {
			this.commandsList.AddCommand (command);
		}
	}

	#endregion

	#region CommandProcessor implementation

	public bool CheckReady (Commands commands, int frame)
	{
		return true;
	}

	public void Process (Command command, int frame)
	{
		Debug.Log (command);
	}

	#endregion

	void Update()
	{
		bool leftDown = Input.GetKey (KeyCode.A);
		bool rightDown = Input.GetKey (KeyCode.D);

		float x = 0.0f;

		if (leftDown)
			x = -1.0f;
		else if (rightDown)
			x = 1.0f;

		if (Mathf.Abs (x) > 0.0f) {
			gameLogic.commandsQueue.EnqueueCommand(new CommandMovement () {
				ProcessFrame = lockstep.GetNextLockstepFrame(),
				direction = new Vector2 (x, 0)
			});
		} else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) {
			gameLogic.commandsQueue.EnqueueCommand(new CommandMovementStop () {
				ProcessFrame = lockstep.GetNextLockstepFrame()
			});
		}

		lockstep.Update (Time.deltaTime);
	}
}
