using UnityEngine;

public class CharacterController : MonoBehaviour {

	CharacterControllerInput input;
	CharacterModel model;
	CharacterMovement movement;

	// Use this for initialization
	void Start () {
		input = GetComponent<CharacterControllerInput> ();
		model = GetComponent<CharacterModel> ();
		movement = GetComponent<CharacterMovement> ();
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 movementDirection = input.GetDirection ();

		movement.Move (movementDirection);

		if (movement.IsMoving ()) {
			model.SetLookingDirection (movement.GetVelocity());
			model.Run ();
		} else {
			model.Idle ();
		}
	}
}
