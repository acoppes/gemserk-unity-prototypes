using UnityEngine;

public class DragForce : MonoBehaviour {

	public Rigidbody2D targetBody;

	public Vector3 startPosition;

	public float maxImpulse = 50.0f;

//	public float minDistance;

	public float maxDistance = 200.0f;

	public AnimationCurve impulseCurveMultiplier;

	public ChargeForceIndicator forceIndicator;

	public Camera mainCamera;

	bool chargingJump = true;

	public Jumper jumper;

	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown (0)) {

			chargingJump = jumper.CanJump();

			if (chargingJump) {
				startPosition = Input.mousePosition;
				forceIndicator.Show (targetBody.position);

//			forceIndicator.Show ((Vector2) mainCamera.ScreenToWorldPoint (startPosition));

				Time.timeScale = 0.0f;
			}

		} else if (Input.GetMouseButton (0)) {
			if (chargingJump) {
				Vector2 difference = mainCamera.ScreenToWorldPoint(startPosition) - mainCamera.ScreenToWorldPoint(Input.mousePosition);

				Vector2 direction = difference.normalized;

	//			if (difference.magnitude > maxDistance) {
	//				difference = difference.normalized * maxDistance;
	//			}

				forceIndicator.UpdateForce (direction, 1.0f);

	//			forceIndicator.UpdateForce (targetBody.position - difference);

	//			forceIndicator.UpdateForce ((Vector2)mainCamera.ScreenToWorldPoint (Input.mousePosition));
			}
		} else if (Input.GetMouseButtonUp (0)) {

			if (chargingJump) {
				Time.timeScale = 1.0f;

				var endPosition = Input.mousePosition;

				Vector2 difference = startPosition - endPosition;
				Vector2 direction = difference.normalized;

				float t = difference.magnitude / maxDistance;
				float multiplier = impulseCurveMultiplier.Evaluate (t);

				targetBody.velocity = Vector2.zero;
				targetBody.AddForce (direction * maxImpulse * multiplier, ForceMode2D.Impulse);

				forceIndicator.Hide ();
			}
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			targetBody.velocity = Vector2.zero;
		}

	}
}
