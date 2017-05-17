using UnityEngine;

public class JumperContacts 
{
	readonly ContactPoint2D[] contacts = new ContactPoint2D[5];

	int contactsCount;

	public void UpdateContacts(Rigidbody2D body)
	{
		contactsCount = body.GetContacts (contacts);
	}

	public int GetContactsCount()
	{
		return contactsCount;
	}

	public ContactPoint2D GetContact(int index)
	{
		return contacts [index];
	}
}

public class Jumper : MonoBehaviour
{
	public Rigidbody2D targetBody;
	public SpriteRenderer unitRenderer;

	bool canJump = true;

	public bool considerContactsToJump;



	public float wallFallForceMultiplier = 1.0f;

	bool contactWithWall;

	public bool fallSlowInWalls = true;

	readonly JumperContacts contacts = new JumperContacts();

	public JumperContacts GetContacts ()
	{
		return contacts;
	}

	public bool CanJump()
	{
		return canJump;
	}

	void FixedUpdate()
	{
		canJump = true;

		contactWithWall = false;

		if (considerContactsToJump) {
			contacts.UpdateContacts (targetBody);

			int contactsWithCeil = 0;

			for (int i = 0; i < contacts.GetContactsCount(); i++) {
				var contact = contacts.GetContact (i);

				contactWithWall = Mathf.Abs (contact.normal.x) > 0.02f;

				if (contact.normal.y < 0)
					contactsWithCeil++;
			}

			canJump = contacts.GetContactsCount() > 0 && contactsWithCeil == 0;

			// Debug.Log (string.Format("contacts: {0}, onWall: {1}", contactCount, contactWithWall));
		}

		unitRenderer.color = canJump ? Color.white : Color.grey;

		if (contactWithWall) {

			if (targetBody.velocity.y < 0 && fallSlowInWalls) {
				// only if falling
				var direction = new Vector2 (0.0f, 1.0f);
				targetBody.AddForce (direction * targetBody.mass * -targetBody.velocity.y * wallFallForceMultiplier, ForceMode2D.Force);
			}
		} 
	}

//	void LateUpdate()
//	{
//
//	}
}
