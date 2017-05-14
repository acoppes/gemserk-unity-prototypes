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

	public bool chargeIfJumpingAndTouch = false;

	public Jumper jumper;

	void StartJumpingMode()
	{
		chargingJump = jumper.CanJump();

		if (chargingJump) {
			startPosition = Input.mousePosition;
			forceIndicator.Show (targetBody.position);

			Time.timeScale = 0.0f;
		}
	}

	void UpdateJumpingMode()
	{
		if (!chargingJump && chargeIfJumpingAndTouch) {
			StartJumpingMode ();
		}

		if (!chargingJump)
			return;

		Vector2 difference = mainCamera.ScreenToWorldPoint (startPosition) - mainCamera.ScreenToWorldPoint (Input.mousePosition);
		Vector2 direction = difference.normalized;
		forceIndicator.UpdateForce (direction, 1.0f);
	}

	void PerformJump()
	{
		if (!chargingJump)
			return;
		
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

	bool IsJumpPressed()
	{
		if (Application.isMobilePlatform) {
			var touchCount = Input.touchCount;
			if (touchCount == 0)
				return false;

			var touchPhase = Input.GetTouch (0).phase;

			return (touchPhase == TouchPhase.Moved) || (touchPhase == TouchPhase.Stationary);
		} else {
			return Input.GetMouseButton (0);
		}	
	}

	bool wasJumpPressed;

	// Update is called once per frame
	void Update () {
	
		bool isJumpPresed = IsJumpPressed ();

		if (isJumpPresed && !wasJumpPressed) {
			StartJumpingMode ();
		} else if (!isJumpPresed && wasJumpPressed) {
			PerformJump ();
		} else {
			UpdateJumpingMode ();
		}

		wasJumpPressed = isJumpPresed;

//		if (Input.GetMouseButtonDown (0)) {
//			StartJumpingMode ();
//		} else if (Input.GetMouseButton (0)) {
//			UpdateJumpingMode ();
//		} else if (Input.GetMouseButtonUp (0)) {
//			PerformJump ();
//		}
//
		if (Input.GetKeyUp (KeyCode.Space)) {
			targetBody.velocity = Vector2.zero;
		}

	}
}
