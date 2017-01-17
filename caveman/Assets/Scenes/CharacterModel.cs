using UnityEngine;

public class CharacterModel : MonoBehaviour {

	public GameObject model;
	public Animator animator;

	public string idleState;
	public string runState;

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
}
