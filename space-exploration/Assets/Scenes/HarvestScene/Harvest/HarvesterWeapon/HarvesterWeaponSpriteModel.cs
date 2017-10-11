using UnityEngine;

public class HarvesterWeaponSpriteModel : HarvesterWeaponModel
{
	public static float Vector2AngleRad(Vector2 v)
	{
		return Mathf.Atan2(v.y, v.x);
	}

	public static float Vector2AngleDeg(Vector2 v)
	{
		return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
	}

	public SpriteRenderer spriteRenderer;

	HarvestBehaviour _harvester;
	Harvestable _harvestable;

	Transform _modelTransform;

	void Awake()
	{
		_modelTransform = spriteRenderer.transform;
	}

	public override void StartHarvesting (HarvestBehaviour harvester, Harvestable harvestable)
	{
		_harvester = harvester;
		_harvestable = harvestable;

		spriteRenderer.enabled = true;
	}

	public override void StopHarvesting ()
	{
		_harvester = null;
		_harvestable = null;

		spriteRenderer.enabled = false;
	}

	void LateUpdate()
	{
		if (_harvester == null || _harvestable == null)
			return;

		var p0 = (Vector2) _harvester.positionReference.position;
		var p1 = (Vector2) _harvestable.positionReference.position;

		var diff = (p1 - p0);

		var m = diff.magnitude;

		var localScale = _modelTransform.localScale;
		localScale.x = m;
		_modelTransform.localScale = localScale;

		_modelTransform.position = p0;
		_modelTransform.localEulerAngles = new Vector3 (0, 0, Vector2AngleDeg(diff));
	}
}