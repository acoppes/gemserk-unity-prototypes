using UnityEngine;
using Gemserk.Lockstep;

public class CommandsScript : MonoBehaviour
{
	Commands commands;

	public Commands Commands {
		get {
			return commands;
		}
		set {
			commands = value;
		}
	}

	void Awake()
	{
		commands = new CommandsList ();
	}
}
