using UnityEngine;

public static class Vector2ExtensionMethods
{
	public static float GetAngle(Vector2 v1, Vector2 v2)
	{
		var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
		return Vector2.Angle(v1, v2) * sign;
	}
}
