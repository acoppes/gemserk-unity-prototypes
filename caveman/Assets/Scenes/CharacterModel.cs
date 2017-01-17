using UnityEngine;

public class CharacterModel : MonoBehaviour {

	public Animator animator;

	public string idleState;
	public string runState;

	public void Run()
	{
		animator.Play (runState);
	}

	public void Idle()
	{
		animator.Play (idleState);
	}
}
