using UnityEngine;

[CreateAssetMenu(menuName="Gemserk/ControlConfig")]
public class ControlConfig : ScriptableObject
{
	public float accelerateForce = 800.0f;

	public float torqueForce = 1000.0f;

	public AnimationCurve accelerateCurve;

	public AnimationCurve rotateCurve;

	public float maxLinearVelocity = 10000.0f;

	public float maxAngularVelocity = 10000.0f;

}
