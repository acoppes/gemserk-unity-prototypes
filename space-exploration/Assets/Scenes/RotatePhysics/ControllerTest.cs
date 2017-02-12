using UnityEngine;

public class ControllerTest : MonoBehaviour {

	public Rigidbody2D body;

	public float accelerateForce = 10.0f;

	public float brakeForce = 100.0f;

	public float torqueForce = 1.0f;

	public ParticleSystem[] particleSystems;

	void Update()
	{
		if (Input.GetKey (KeyCode.W)) {
			body.AddRelativeForce (new Vector2 (0, accelerateForce));

			for (int i = 0; i < particleSystems.Length; i++) {
				var emission = particleSystems [i].emission;
				emission.enabled = true;
			}


		} else if (Input.GetKey (KeyCode.S)) {
			body.AddRelativeForce (new Vector2 (0, -brakeForce));
			for (int i = 0; i < particleSystems.Length; i++) {
				var emission = particleSystems [i].emission;
				emission.enabled = false;
			}
		} else {
			for (int i = 0; i < particleSystems.Length; i++) {
				var emission = particleSystems [i].emission;
				emission.enabled = false;
			}
		}


		if (Input.GetKey (KeyCode.A)) {
			body.AddTorque (torqueForce, ForceMode2D.Force);
		} else if (Input.GetKey (KeyCode.D)) {
			body.AddTorque (-torqueForce, ForceMode2D.Force);
		}
	}

}
