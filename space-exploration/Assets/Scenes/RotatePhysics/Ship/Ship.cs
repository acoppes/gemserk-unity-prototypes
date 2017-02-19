using UnityEngine;

public class Ship : MonoBehaviour {

	public ControlConfig control;
	public Rigidbody2D body;

	[SerializeField]
	Vector3 lookingDirection;

	[SerializeField]
	float angle;

	public float movementEpsilon = 0.1f;

	Vector2 desiredDirection;

	public void SetDesiredDirection(Vector2 desiredDirection)
	{
		this.desiredDirection = desiredDirection;	
	}

	void FixedUpdate()
	{
		lookingDirection = body.transform.up;

		if (desiredDirection.SqrMagnitude() < movementEpsilon)
			return;

		angle = Vector2.Angle (desiredDirection, lookingDirection);
		float factor = 1.0f - (angle / 180.0f);

		float signedAngle = Vector2ExtensionMethods.GetAngle (desiredDirection, lookingDirection);

		//		float rotateFactor = angle / 180.0f;

		var rotateFactor = control.rotateCurve.Evaluate (factor);
		var accelerateFactor = control.accelerateCurve.Evaluate (factor) * desiredDirection.magnitude;

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
		Gizmos.DrawLine (body.transform.position, transform.position + d);
	}

}
