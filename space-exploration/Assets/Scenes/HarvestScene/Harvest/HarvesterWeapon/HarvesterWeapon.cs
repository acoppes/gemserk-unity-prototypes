using UnityEngine;

public class HarvesterWeapon : MonoBehaviour {

	HarvesterWeaponModel _model;

	public float speed;

	HarvestBehaviour _harvester;
	Harvestable _harvestable;

	void Awake()
	{
		_model = GetComponent<HarvesterWeaponModel> ();
	}

	public void StartHarvesting(HarvestBehaviour harvester, Harvestable harvestable)
	{
		_harvester = harvester;
		_harvestable = harvestable;

		if (_model != null)
			_model.StartHarvesting (harvester, harvestable);
	}

	public void StopHarvesting()
	{
		_harvester = null;
		_harvestable = null;

		if (_model != null)
			_model.StopHarvesting ();
	}

	void FixedUpdate()
	{
		if (_harvester == null || _harvestable == null)
			return;
		_harvestable.Harvest (speed * Time.deltaTime);
		if (_harvestable.IsDepleted ())
			_model.StopHarvesting ();
	}

	public bool IsDone()
	{
		if (_harvester == null || _harvestable == null)
			return true;
		return _harvestable.IsDepleted ();
	}

}
