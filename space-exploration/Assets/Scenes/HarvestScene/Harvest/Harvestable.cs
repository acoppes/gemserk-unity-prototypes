using UnityEngine;

public class Harvestable : MonoBehaviour {

	public float total;
	public float current;

	public GameObject positionReference;

	const float epsilon = 0.01f;

	public bool IsDepleted ()
	{
		return current < epsilon;
	}
}


