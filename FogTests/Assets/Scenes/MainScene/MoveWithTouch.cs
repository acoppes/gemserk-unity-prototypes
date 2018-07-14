using UnityEngine;

public class MoveWithTouch : MonoBehaviour
{
	[SerializeField]
	protected Camera _referenceCamera;

	void Update () {

		if (Input.GetMouseButton(0))
		{
			var v3 = Input.mousePosition;
			v3 = _referenceCamera.ScreenToWorldPoint(v3);
			transform.position = v3;
		}
	}
}
