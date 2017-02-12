using UnityEngine;

public class ControllerTest : MonoBehaviour {

	public InputAxisProcessor verticalAxis;
	public InputAxisProcessor horizontalAxis;

	public Rigidbody2D body;

	public float accelerateForce = 10.0f;

	public float brakeForce = 100.0f;

	public float torqueForce = 1.0f;

	void Update()
	{
		verticalAxis.Update ();
		horizontalAxis.Update ();

		if (verticalAxis.Value > 0) {
			body.AddRelativeForce (new Vector2 (0, accelerateForce));

//			for (int i = 0; i < particleSystems.Length; i++) {
//				var emission = particleSystems [i].emission;
//				emission.enabled = true;
//			}


		} else if (verticalAxis.Value < 0) {
			body.AddRelativeForce (new Vector2 (0, -brakeForce));
		} 
			
		if (horizontalAxis.Value < 0) {
			body.AddTorque (torqueForce, ForceMode2D.Force);
		} else if (horizontalAxis.Value > 0) {
			body.AddTorque (-torqueForce, ForceMode2D.Force);
		}
	}

}
