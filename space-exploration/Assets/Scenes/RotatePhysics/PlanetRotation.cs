using UnityEngine;

public class PlanetRotation : MonoBehaviour {

	public float rotationSpeed;

	void Update()
	{
		var eulerAngles = transform.localEulerAngles;
		eulerAngles.z = eulerAngles.z + rotationSpeed * Time.deltaTime;
		transform.localEulerAngles = eulerAngles;
	}
}
