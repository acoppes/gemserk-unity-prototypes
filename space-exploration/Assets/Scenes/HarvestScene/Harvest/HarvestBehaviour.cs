using UnityEngine;

public class HarvestBehaviour : MonoBehaviour {

	public float range;
	public GameObject positionReference;

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere (transform.position, range);
	}

}
