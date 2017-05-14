using UnityEngine;
using Gemserk.Lockstep;

public class DeterministicEngine : MonoBehaviour {

	GameFixedUpdate deterministicUpdate;

	GameLogicScript gameLogic;
	CommandsScript commandsScript;

	void Awake()
	{
		commandsScript = GetComponent<CommandsScript> ();

//		LockstepLogic lockstepLogic = new CommandsListLockstepLogic (commandsList, commandsProcessor);
		deterministicUpdate = new GameFixedUpdate ();

//		deterministicUpdate.GameFramesPerLockstep = 1;
		deterministicUpdate.FixedStepTime = 16.0f / 1000.0f;

		gameLogic = GetComponent<GameLogicScript> ();

		deterministicUpdate.SetGameLogic (gameLogic);

		deterministicUpdate.Init ();

		// to start executing...
//		commandsList.AddCommand (new CommandBase () { 
//			ProcessFrame = deterministicUpdate.GetFirstLockstepFrame()
//		});

//		gameLogic.Commands = commands;

		Init ();
	}

	void Init()
	{
		var characterBuilders = GetComponentsInChildren<CharacterBuildScript> ();
		foreach (var characterBuilder in characterBuilders) {
			characterBuilder.CreateLogic (gameLogic);
		}
	}

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
			commandsScript.Commands.AddCommand(new CommandMovement () {
				ProcessFrame = deterministicUpdate.CurrentGameFrame + 1,
				direction = new Vector2 (x, 0)
			});
		} else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) {
			commandsScript.Commands.AddCommand(new CommandMovementStop () {
				ProcessFrame = deterministicUpdate.CurrentGameFrame + 1
			});
		}

		deterministicUpdate.Update (Time.deltaTime);
	}
}
