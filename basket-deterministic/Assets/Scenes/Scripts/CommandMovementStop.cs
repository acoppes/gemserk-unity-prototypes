using UnityEngine;
using Gemserk.Lockstep;

public class CommandMovementStop : CommandBase
{
	public override string ToString ()
	{
		return string.Format ("[CommandMovementStop]");
	}
}
