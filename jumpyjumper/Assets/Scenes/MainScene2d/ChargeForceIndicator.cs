using UnityEngine;

public class ChargeForceIndicator: MonoBehaviour
{
	public Jumper jumper;

	public LineRenderer lineRenderer;

	Vector2 startPosition;

	public float maxLength = 5.0f;

	public bool inversedForce = true;

	public float positionZ = 1;

	public Color jumpEnabledColor = Color.blue;
	public Color jumpDisabledColor = Color.grey;

	void Start()
	{
		lineRenderer.enabled = false;
	}

	public void Show(Vector2 startPosition)
	{
		this.startPosition = startPosition;

		lineRenderer.enabled = true;
		lineRenderer.SetPosition (0, startPosition.ToVector3(positionZ));
		lineRenderer.SetPosition (1, startPosition.ToVector3(positionZ));
	}

	public void UpdateForce(Vector2 direction, float forceFactor) 
	{
		float inversedFactor = inversedForce ? -1.0f : 1.0f;

		lineRenderer.SetPosition (0, startPosition.ToVector3(positionZ));
		var destination = startPosition + direction * forceFactor * maxLength * inversedFactor;
		lineRenderer.SetPosition (1, destination.ToVector3(positionZ));

		bool isJumpBlocked = jumper.IsJumpBlocked ();

		if (isJumpBlocked) {
			lineRenderer.startColor = jumpDisabledColor;
			lineRenderer.endColor = jumpDisabledColor;
		} else {
			lineRenderer.startColor = jumpEnabledColor;
			lineRenderer.endColor = jumpEnabledColor;		
		}
	}

	public void Hide()
	{
		lineRenderer.enabled = false;
	}
}
