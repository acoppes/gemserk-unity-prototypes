using UnityEngine;

public class ChargeForceIndicator: MonoBehaviour
{
	public Jumper jumper;

	public LineRenderer lineRenderer;

	Vector2 startPosition;

	public float maxLength = 5.0f;

	public bool inversedForce = true;

	public float positionZ = 1;

	public float maxContactAngle = 0.75f;

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

		var contacts = jumper.GetContacts ();

		bool limitedAngle = false;

		for (int i = 0; i < contacts.GetContactsCount(); i++) {
			var contact = contacts.GetContact (i);
			var normal = contact.normal;

			var dot = Vector2.Dot (normal, direction);

			Debug.Log ("dot: " + dot);

			if (dot < maxContactAngle) {
				limitedAngle = true;
				break;
			}

		}

		if (limitedAngle) {
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
