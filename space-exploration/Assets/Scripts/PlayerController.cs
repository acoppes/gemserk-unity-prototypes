using UnityEngine;

public class PlayerController : MonoBehaviour {

	public ShipInput ShipInput {
		get;
		set;
	}

	public ShipController ShipController {
		get;
		set;
	}

	float epsilon = 0.01f;

	void Awake()
	{
		ShipInput = GetComponentInChildren<ShipInput> ();
		ShipController = GetComponentInChildren<ShipController> ();
	}

	public void Update()
	{
		if (ShipInput.RotateDirection() < -epsilon ) {
			ShipController.TurnLeft ();
		} else if (ShipInput.RotateDirection() > epsilon) {
			ShipController.TurnRight ();
		}

		if (ShipInput.AcceleratePressed ()) {
			ShipController.Accelerate ();
		}
	}

}