using UnityEngine;

public class SmoothFollowObject : MonoBehaviour {

	public Transform followingObject;

	public float factor = 100.0f;

	void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, followingObject.position, Time.deltaTime * factor);
//		transform.rotation = Quaternion.Lerp(transform.rotation, playerMarker.rotation, Time.deltaTime * 100);
	}

}
