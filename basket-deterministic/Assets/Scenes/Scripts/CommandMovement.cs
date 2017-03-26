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
