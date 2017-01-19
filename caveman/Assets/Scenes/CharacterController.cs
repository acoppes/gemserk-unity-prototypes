using UnityEngine;

public class CharacterController : MonoBehaviour {

	CharacterControllerInput input;
	CharacterModel model;
	CharacterMovement movement;

	public float punchReload = 1.0f;
	float punchLastTime;

	public bool stopOnPunch = true;

	// Use this for initialization
	void Start () {
		input = GetComponent<CharacterControllerInput> ();
		model = GetComponent<CharacterModel> ();
		movement = GetComponent<CharacterMovement> ();
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 movementDirection = input.GetDirection ();

		var isPunching = model.IsPunching ();

		if (!isPunching || !stopOnPunch)
			movement.Move (movementDirection);

		if (model.IsPunching ())
			return;

		// cant do other thing while jumping?
		if (model.IsJumping ())
			return;

		if (movement.IsMoving ()) {
			model.SetLookingDirection (movement.GetVelocity ());
			model.Run ();
		} else {
			model.Idle ();
		}

		if (input.WasPunchPressed ()) {
		
			if (Time.realtimeSinceStartup - punchLastTime > punchReload) {
				model.Punch ();
				punchLastTime = Time.realtimeSinceStartup;
			}
		
		}

		if (input.IsJumpPressed ()) {
			model.Jump ();
		}
	}
}
