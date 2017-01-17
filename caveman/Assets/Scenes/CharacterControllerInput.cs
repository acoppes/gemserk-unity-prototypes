using UnityEngine;

public class CharacterControllerInput : MonoBehaviour {

	public string horizontalAxisName;
	public string verticalAxisName;

	public string punchButtonName;

	Vector2 direction;

	bool punchPressed;

	public Vector2 GetDirection()
	{
		return direction;
	}

	public bool WasPunchPressed()
	{
		return punchPressed;
	}

	// Update is called once per frame
	void Update () {

		direction.Set (0, 0);

		direction.x = Input.GetAxis (horizontalAxisName);
		direction.y = Input.GetAxis (verticalAxisName);
	
		punchPressed = Input.GetButtonDown (punchButtonName);
	}
}
