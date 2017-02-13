using UnityEngine;

public class OtherControllerTest : MonoBehaviour {

	public InputAxisProcessor verticalAxis;
	public InputAxisProcessor horizontalAxis;

	public Ship ship;

	// Update is called once per frame
	void Update () {

		verticalAxis.Update ();
		horizontalAxis.Update ();

		var desiredDirection = new Vector2(horizontalAxis.Value, verticalAxis.Value);

		ship.SetDesiredDirection (desiredDirection);
	}

}
