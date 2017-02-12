using UnityEngine;
using UnityEngine.Serialization;

public class OtherControllerTest : MonoBehaviour {

	public InputAxisProcessor verticalAxis;
	public InputAxisProcessor horizontalAxis;

	public Rigidbody2D body;

//	public float accelerateForce = 500.0f;

//	public float torqueForce = 500.0f;

	[SerializeField]
	Vector2 desiredDirection;

	[SerializeField]
	Vector3 lookingDirection;

	[SerializeField]
	float angle;

//	public AnimationCurve accelerateCurve;

//	[FormerlySerializedAs("curve")]
//	public AnimationCurve rotateCurve;

	public ControlConfig control;

	private static float GetAngle(Vector2 v1, Vector2 v2)
	{
		var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
		return Vector2.Angle(v1, v2) * sign;
	}

	// Update is called once per frame
	void Update () {

		lookingDirection = body.transform.up;
	
		verticalAxis.Update ();
		horizontalAxis.Update ();

		desiredDirection = new Vector2(horizontalAxis.Value, verticalAxis.Value);

		if (desiredDirection.SqrMagnitude() < 0.1f)
			return;

		angle = Vector2.Angle (desiredDirection, lookingDirection);
		float factor = 1.0f - (angle / 180.0f);

		float signedAngle = GetAngle (desiredDirection, lookingDirection);

//		float rotateFactor = angle / 180.0f;

		var rotateFactor = control.rotateCurve.Evaluate (factor);
		var accelerateFactor = control.accelerateCurve.Evaluate (factor);

		if (signedAngle > 1f) {
			body.AddTorque (-control.torqueForce * rotateFactor, ForceMode2D.Force);
		} else if (signedAngle < 1f) {
			body.AddTorque (control.torqueForce * rotateFactor, ForceMode2D.Force);
		} 

		body.AddRelativeForce (new Vector2 (0, accelerateFactor * control.accelerateForce));

		var currentAngularVelocity = body.angularVelocity;
		if (Mathf.Abs(currentAngularVelocity) > control.maxAngularVelocity) {
			body.angularVelocity = control.maxAngularVelocity * Mathf.Sign (currentAngularVelocity);
		}

		var linearVelocity = body.velocity;
		if (linearVelocity.SqrMagnitude() > control.maxLinearVelocity * control.maxLinearVelocity) {
			body.velocity = linearVelocity.normalized * control.maxLinearVelocity;
		}

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Vector3 d = desiredDirection * 40.0f;
		Gizmos.DrawLine (body.transform.position, body.transform.position + d);
	}
}
