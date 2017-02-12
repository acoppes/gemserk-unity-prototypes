using UnityEngine;

public class ShipInputBehaviour : MonoBehaviour, ShipInput {

	public InputAxisProcessor rotateDirection;
	public InputPressedProcessor accelerate;

	#region ShipInput implementation

	public float RotateDirection()
	{
		return rotateDirection.Value;
	}

	public bool AcceleratePressed ()
	{
		return accelerate.Value;
	}
	#endregion

	void Update()
	{
		rotateDirection.Update ();
		accelerate.Update ();
	}

}