using UnityEngine;
using System;
using System.Collections.Generic;

public class HarvestBehaviour : MonoBehaviour {

	public float range;
	public GameObject positionReference;

	[NonSerialized]
	public World world;

	HarvestSystem _harvestSystem;

	Harvestable targetedHarvestable;

	List<Harvestable> harvestablesInRange = new List<Harvestable>();

	void Start()
	{
		_harvestSystem = world.GetComponent<HarvestSystem>();
		if (_harvestSystem != null)
			_harvestSystem.Register (this);
	}

	void OnDestroy()
	{
		if (_harvestSystem != null)
			_harvestSystem.Unregister(this);
	}

	public void ProcessHarvestables (List<Harvestable> harvestables)
	{
		harvestablesInRange.Clear ();

		foreach (var harvestable in harvestables) {
			if (IsInRange (harvestable))
				harvestablesInRange.Add (harvestable);
		}

		if (targetedHarvestable == null) {
			foreach (var harvestable in harvestablesInRange) {
				if (harvestable.IsDepleted ())
					continue;
				StartHarvesting (harvestable);
				break;
			}
		} else {
			Harvest ();
		}
	}

	void StartHarvesting(Harvestable harvestable)
	{
		targetedHarvestable = harvestable;
		// turn on ray..., configure...
	}

	void StopHarvesting()
	{
		targetedHarvestable = null;
		// turn off ray...
	}

	void Harvest ()
	{
		// harverst logic...

		if (targetedHarvestable.IsDepleted ()) {
			StopHarvesting ();
		}
	}

	bool IsInRange(Harvestable harvestable)
	{
		return Vector2.SqrMagnitude(harvestable.transform.position - transform.position) < range * range;
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere (transform.position, range);
	}

}
