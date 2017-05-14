using UnityEngine;

public static class UnityExtensions
{
	public static Vector3 ToVector3(this Vector2 v, float z)
	{
		return new Vector3 (v.x, v.y, z);
	}
}

public class ChargeForceIndicator: MonoBehaviour
{
	public LineRenderer lineRenderer;

	Vector2 startPosition;

	public float maxLength = 5.0f;

	public bool inversedForce = true;

	public float positionZ = 1;

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
	}

	public void Hide()
	{
		lineRenderer.enabled = false;
	}
}
