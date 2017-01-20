using UnityEngine;

public class SimpleCameraPan : MonoBehaviour {

	bool wasPressed = false;

	Vector3 mouseLastPosition;

	public float multiplier = 1.0f;

	void Update()
	{
		if (Input.GetMouseButton (1)) {
	
			if (!wasPressed) {
				mouseLastPosition = Input.mousePosition;
				wasPressed = true;
			} else {

				var currentPosition = Input.mousePosition;
				var deltaPosition = currentPosition - mouseLastPosition;
			
				transform.localPosition = transform.localPosition + deltaPosition * multiplier;

				mouseLastPosition = currentPosition;
			}
		} else {
			wasPressed = false;
		}
	}
}
