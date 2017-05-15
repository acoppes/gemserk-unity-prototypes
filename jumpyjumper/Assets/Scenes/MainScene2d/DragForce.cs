using UnityEngine;

public class DragForce : MonoBehaviour {

	public Rigidbody2D targetBody;

	public Vector3 startPosition;

	public float maxImpulse = 50.0f;

//	public float minDistance;

	public float maxDistance = 200.0f;

	public AnimationCurve impulseCurveMultiplier;

	public ChargeForceIndicator forceIndicator;
	public ChargeForceIndicator touchForceIndicator;

	public Camera mainCamera;

	bool canJump = true;

	public bool chargeIfJumpingAndTouch = false;

	public Jumper jumper;

	void StartJumpingMode()
	{
		canJump = jumper.CanJump();

		if (canJump) {
			if (forceIndicator != null)
				forceIndicator.Show (targetBody.position);

			if (touchForceIndicator != null) {
				touchForceIndicator.Show(mainCamera.ScreenToWorldPoint(startPosition));
			}

			Time.timeScale = 0.0f;
		}
	}

	void UpdateJumpingMode()
	{
		if (!canJump && chargeIfJumpingAndTouch) {
			StartJumpingMode ();
		}

		if (!canJump)
			return;

		Vector2 difference = mainCamera.ScreenToWorldPoint (startPosition) - mainCamera.ScreenToWorldPoint (Input.mousePosition);
		Vector2 direction = difference.normalized;
		if (forceIndicator != null)
			forceIndicator.UpdateForce (direction, 1.0f);

		if (touchForceIndicator != null) {
			touchForceIndicator.UpdateForce(direction, 1.0f);
		}

	}

	void PerformJump()
	{
		if (!canJump)
			return;
		
		Time.timeScale = 1.0f;

		var endPosition = Input.mousePosition;

		Vector2 difference = startPosition - endPosition;
		Vector2 direction = difference.normalized;

		float t = difference.magnitude / maxDistance;
		float multiplier = impulseCurveMultiplier.Evaluate (t);

		targetBody.velocity = Vector2.zero;
		targetBody.AddForce (direction * maxImpulse * multiplier, ForceMode2D.Impulse);

		if (forceIndicator != null)
			forceIndicator.Hide ();

		if (touchForceIndicator != null) {
			touchForceIndicator.Hide ();
		}
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
			startPosition = Input.mousePosition;
			StartJumpingMode ();
		} else if (!isJumpPresed && wasJumpPressed) {
			PerformJump ();
		} else if (isJumpPresed && wasJumpPressed) {
			UpdateJumpingMode ();
		}

		wasJumpPressed = isJumpPresed;

		if (Input.GetKeyUp (KeyCode.Space)) {
			targetBody.velocity = Vector2.zero;
		}

	}
}
