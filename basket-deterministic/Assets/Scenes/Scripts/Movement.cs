using UnityEngine;
using Gemserk.Lockstep;

public class Movement : GameLogic
{
	Vector2 direction;

	bool moving;

	public float speed;

	GameObject model;

	public Movement (GameObject model)
	{
		this.model = model;
	}

	public void Move (Vector2 direction)
	{
		moving = true;
		this.direction = direction;
	}

	public void Stop()
	{
		moving = false;
	}

	#region GameLogic implementation

	public void GameUpdate (float dt, int frame)
	{
		if (moving)
			model.transform.position = model.transform.position + new Vector3 (direction.x * speed * dt, 0, 0);
	}

	#endregion
}
