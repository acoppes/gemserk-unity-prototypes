using UnityEngine;

public class CharacterBuildScript : MonoBehaviour
{
	public GameObject model;

	public void CreateLogic(GameLogicScript logicScript)
	{
		var commandsScript = FindObjectOfType<CommandsScript> ();
		var movement = new Movement (model);

		movement.speed = 1.0f;

		logicScript.GameLogics.Add (movement);
		logicScript.GameLogics.Add (new Character (commandsScript.Commands, movement));

	}

}
