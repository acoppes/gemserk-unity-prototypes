using UnityEngine;

public class HarvestRayBehaviour : MonoBehaviour {

	public float speed;

	HarvestBehaviour _harvester;
	Harvestable _harvestable;

	public void StartHarvesting(HarvestBehaviour harvester, Harvestable harvestable)
	{
		_harvester = harvester;
		_harvestable = harvestable;
	}

	public void StopHarvesting()
	{
		_harvester = null;
		_harvestable = null;
	}

	void FixedUpdate()
	{
		if (_harvester == null || _harvestable == null)
			return;
		_harvestable.Harvest (speed * Time.deltaTime);
	}

	public bool IsDone()
	{
		if (_harvester == null || _harvestable == null)
			return true;
		return _harvestable.IsDepleted ();
	}

}
