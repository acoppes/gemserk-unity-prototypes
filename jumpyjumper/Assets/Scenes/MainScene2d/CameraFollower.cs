using UnityEngine;

public class CameraFollower : MonoBehaviour {

	public Transform targetTransform;

	public float lerpSpeed = 1.0f;

	public float minDistance = 0.1f;
	
	// Update is called once per frame
	void FixedUpdate () {
		var myposition = transform.position;
		myposition.y = Mathf.Lerp(myposition.y, targetTransform.position.y, lerpSpeed * Time.deltaTime);

		if (Mathf.Abs (myposition.y - targetTransform.position.y) < minDistance) {
			myposition.y = targetTransform.position.y;
		}

		transform.position = myposition;
	}
}
