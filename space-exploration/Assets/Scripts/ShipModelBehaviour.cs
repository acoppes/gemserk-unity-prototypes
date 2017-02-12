using UnityEngine;

public class ShipModelBehaviour : MonoBehaviour, ShipModel {

	#region ShipModel implementation

	public void RotateToAngle (float angle)
	{
		transform.localEulerAngles = new Vector3 (0, 0, -angle);
	}

	public void MoveToPosition (Vector2 position)
	{
		transform.position = position;
	}

	#endregion
	
}
