using UnityEngine;

public class CharacterControllerInput : MonoBehaviour {

	public string horizontalAxisName;
	public string verticalAxisName;

	Vector2 direction;

	public Vector2 GetDirection()
	{
		return direction;
	}

	// Update is called once per frame
	void Update () {

		direction.Set (0, 0);

		direction.x = Input.GetAxis (horizontalAxisName);
		direction.y = Input.GetAxis (verticalAxisName);
	}
}
