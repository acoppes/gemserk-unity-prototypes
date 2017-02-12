using UnityEngine;

public class ShipControllerBehaviour : MonoBehaviour, ShipController {

	public float turnSpeed;

	float turning;

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
		
	}

	#endregion

	void Awake()
	{
		ShipModel = model.GetComponentInChildren<ShipModel> ();	
	}

	void FixedUpdate()
	{
		currentAngle += turning * turnSpeed * Time.deltaTime;
		turning = 0.0f;

		ShipModel.RotateToAngle (currentAngle);
	}


}
