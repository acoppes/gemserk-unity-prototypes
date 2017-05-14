using UnityEngine;

public class Jumper : MonoBehaviour
{
	public Rigidbody2D targetBody;
	public SpriteRenderer unitRenderer;

	bool canJump = true;

	public bool considerContactsToJump;

	readonly ContactPoint2D[] contacts = new ContactPoint2D[5];

	public float wallFallForceMultiplier = 1.0f;

	bool contactWithWall;

	public bool fallSlowInWalls = true;

	public bool CanJump()
	{
		return canJump;
	}

	void FixedUpdate()
	{
		canJump = true;

		contactWithWall = false;

		if (considerContactsToJump) {
			int contactCount = targetBody.GetContacts (contacts);

			for (int i = 0; i < contactCount; i++) {
				var contact = contacts [i];
				contactWithWall = Mathf.Abs (contact.normal.x) > 0.02f;
			}

			canJump = contactCount > 0;

			// Debug.Log (string.Format("contacts: {0}, onWall: {1}", contactCount, contactWithWall));
		}

		unitRenderer.color = canJump ? Color.white : Color.grey;
	}

	void LateUpdate()
	{
		if (contactWithWall) {

			if (targetBody.velocity.y < 0 && fallSlowInWalls) {
				// only if falling
				var direction = new Vector2 (0.0f, 1.0f);
				targetBody.AddForce (direction * targetBody.mass * -targetBody.velocity.y * wallFallForceMultiplier, ForceMode2D.Force);
			}
		} 
	}
}
