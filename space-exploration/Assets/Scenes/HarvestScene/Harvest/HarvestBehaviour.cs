﻿using UnityEngine;
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
		if (targetedHarvestable != null) {
			return;
		}

		harvestablesInRange.Clear ();

		foreach (var harvestable in harvestables) {
			if (IsInRange (harvestable))
				harvestablesInRange.Add (harvestable);
		}
	}

	void FixedUpdate()
	{
		if (targetedHarvestable == null) {
//			var harvestables = world.GetComponent<Harvestables> ();
//			if (harvestables == null)
//				return;
//			var harvestableList = harvestables.GetHarvestables ();
			foreach (var harvestable in harvestablesInRange) {
				if (harvestable.IsDepleted ())
					continue;
				StartHarvesting(harvestable);
				break;
			}
		} else {
			
		}
	}

	void StartHarvesting(Harvestable harvestable)
	{
		targetedHarvestable = harvestable;
		// turn on ray..., configure...
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
