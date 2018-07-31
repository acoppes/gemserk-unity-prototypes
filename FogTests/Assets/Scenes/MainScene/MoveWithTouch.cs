using UnityEngine;

public class MoveWithTouch : MonoBehaviour
{
	[SerializeField]
	protected Camera _referenceCamera;

	[SerializeField]
	protected Vision _vision;

	[SerializeField]
	protected VisionSystem _visionSystem;

	void Update () {

		if (Input.GetMouseButton(0))
		{
			var v3 = Input.mousePosition;
			v3 = _referenceCamera.ScreenToWorldPoint(v3);
			transform.position = v3;

			_vision.groundLevel = 0;
			_vision.player = _visionSystem.currentPlayer;
			
			var collider = Physics2D.OverlapPoint(v3);
			if (collider != null)
			{
				var obstacle = collider.GetComponent<VisionObstacle>();
				if (obstacle != null)
				{
					short increment = 1;
					_vision.groundLevel = (short) (obstacle.groundLevel + increment);
				}
			}
			
		}
	}
}
