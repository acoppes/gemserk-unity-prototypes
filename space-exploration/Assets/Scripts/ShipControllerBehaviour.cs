using UnityEngine;

public class ShipControllerBehaviour : MonoBehaviour, ShipController {

	public float turnSpeed;

	public float moveSpeed;

	float turning;
	float accelerating;

	float currentAngle;

	public Transform model;

	public ShipModel ShipModel {
		get;
		set;
	}

	#region ShipController implementation

	public void TurnLeft ()
	{
		turning = -1f;
	}

	public void TurnRight ()
	{
		turning = 1f;
	}

	public void Accelerate ()
	{
		accelerating = 1.0f;
	}

	#endregion

	void Awake()
	{
		ShipModel = model.GetComponentInChildren<ShipModel> ();	
	}

	void FixedUpdate()
	{
		currentAngle += turning * turnSpeed * Time.deltaTime;

		Vector2 position = this.transform.position;

		Vector2 movementDirection = Quaternion.Euler(0, 0, -currentAngle) * new Vector2(0.0f, 1.0f);

		position = position + movementDirection * moveSpeed * accelerating * Time.deltaTime;

		transform.position = position;

		turning = 0.0f;
		accelerating = 0.0f;

		ShipModel.RotateToAngle (currentAngle);
		ShipModel.MoveToPosition (position);
	}


}
