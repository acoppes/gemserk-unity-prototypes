using UnityEngine;

public class MoveWithTouch : MonoBehaviour
{
	[SerializeField]
	protected Camera _referenceCamera;

	[SerializeField]
	protected Vision _vision;

	void Update () {

		if (Input.GetMouseButton(0))
		{
			var v3 = Input.mousePosition;
			v3 = _referenceCamera.ScreenToWorldPoint(v3);
			transform.position = v3;

			_vision.groundLevel = 0;
			
			var collider = Physics2D.OverlapPoint(v3);
			if (collider != null)
			{
				var obstacle = collider.GetComponent<VisionObstacle>();
				if (obstacle != null)
				{
					_vision.groundLevel = obstacle.groundLevel + 1;
				}
			}
			
		}
	}
}
