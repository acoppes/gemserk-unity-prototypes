using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	public float maxSpeed;
	public float acceleration;

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

		if (newVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
			newVelocity = newVelocity.normalized * maxSpeed;
		}

		velocity = newVelocity;

		var newPosition = transform.position + (Vector3) velocity;

		transform.position = newPosition;
	}
}
