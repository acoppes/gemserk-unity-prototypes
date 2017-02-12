using UnityEngine;

public class ShipControllerBehaviour : MonoBehaviour, ShipController {

	public float turnSpeed;

	float turning;

	float currentAngle = 0.0f;

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
		
	}

	#endregion

	void Awake()
	{
		ShipModel = GetComponentInChildren<ShipModel> ();	
	}

	void FixedUpdate()
	{
		currentAngle += turning * turnSpeed * Time.deltaTime;
		turning = 0.0f;

		ShipModel.RotateToAngle (currentAngle);
	}


}
