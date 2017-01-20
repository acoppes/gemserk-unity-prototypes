using UnityEngine;

[ExecuteInEditMode]
public class RoundedTransformReference : MonoBehaviour {

	public Transform reference;

	Vector3 RoundedVector(Vector3 v)
	{
		return new Vector3 (Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
	}

	void LateUpdate()
	{
		if (reference == null)
			return;

		this.transform.localPosition = RoundedVector(reference.localPosition);
		this.transform.localEulerAngles = RoundedVector(reference.localEulerAngles);
		this.transform.localScale = RoundedVector(reference.localScale);
	}

}
