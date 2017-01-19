using UnityEngine;

public class CharacterModel : MonoBehaviour {

	public GameObject model;
	public Animator animator;

	public string idleState;
	public string runState;
	public string punchState;

	public string jumpState;
	public string fallState;

	public bool IsPunching()
	{
		return animator.GetCurrentAnimatorStateInfo (0).IsName (punchState);
	}

	public bool IsJumping()
	{
		return animator.GetCurrentAnimatorStateInfo (0).IsName (jumpState)
			|| animator.GetCurrentAnimatorStateInfo (0).IsName (fallState);
	}

	public void SetLookingDirection(Vector2 lookingDirection)
	{
		var localScale = model.transform.localScale;
		localScale.x = lookingDirection.x > 0 ? 1 : -1;
		model.transform.localScale = localScale;
	}

	public void Run()
	{
		animator.Play (runState);
	}

	public void Idle()
	{
		animator.Play (idleState);
	}

	public void Punch()
	{
		animator.Play (punchState);
	}

	public void Jump()
	{
		animator.Play (jumpState);
	}
}
