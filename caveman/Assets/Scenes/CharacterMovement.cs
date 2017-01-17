using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	public float maxSpeed;
	public float acceleration;

	public float desacceleration;

	Vector2 velocity;

	public Vector2 GetVelocity()
	{
		return velocity;
	}

	public bool IsMoving()
	{
		return velocity.sqrMagnitude > 0;
	}

	public void Move(Vector2 desiredDirection)
	{
		var newVelocity = velocity + desiredDirection * acceleration * Time.deltaTime;

		if (desiredDirection.sqrMagnitude <= 0) {
			if (velocity.sqrMagnitude > 0.001f) {
				Vector2 direction = velocity.normalized;
				direction *= -1;

				newVelocity = velocity + direction * desacceleration * Time.deltaTime;

				if (Vector2.Dot (newVelocity, velocity) < 0.0f) {
					newVelocity.Set (0, 0);
				}

//				if (Mathf.Abs(Mathf.Sign (newVelocity.x) - Mathf.Sign (velocity.x)) < 0.001f) {
//					newVelocity.Set (0, 0);
//				}
			} else {
				newVelocity.Set (0, 0);
			}
		} 

		if (newVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
			newVelocity = newVelocity.normalized * maxSpeed;
		}

		newVelocity.y *= 0.75f;
		velocity = newVelocity;

		var newPosition = transform.position + (Vector3) velocity;

		transform.position = newPosition;
	}
}
