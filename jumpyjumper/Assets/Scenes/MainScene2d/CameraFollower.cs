using UnityEngine;

public class CameraFollower : MonoBehaviour {

	public Transform targetTransform;
	
	// Update is called once per frame
	void Update () {
		var myposition = transform.position;
		myposition.y = targetTransform.position.y;
		transform.position = myposition;
	}
}
