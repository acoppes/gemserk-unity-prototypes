using UnityEngine;

[ExecuteInEditMode]
public class RoundedTransformReference : MonoBehaviour {

	public Transform reference;

	void LateUpdate()
	{
		if (reference == null)
			return;
		
		var referencePosition = reference.localPosition;

		referencePosition.x = Mathf.RoundToInt (referencePosition.x);
		referencePosition.y = Mathf.RoundToInt (referencePosition.y);
		referencePosition.z = Mathf.RoundToInt (referencePosition.z);

		this.transform.localPosition = referencePosition;
		this.transform.localRotation = reference.localRotation;
		this.transform.localScale = reference.localScale;
	}

}
