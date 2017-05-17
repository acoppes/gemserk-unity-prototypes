using UnityEngine;

public static class UnityExtensions
{
	public static Vector3 ToVector3(this Vector2 v, float z)
	{
		return new Vector3 (v.x, v.y, z);
	}
}
