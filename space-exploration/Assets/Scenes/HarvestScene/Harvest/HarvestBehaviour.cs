﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class HarvestBehaviour : MonoBehaviour {

	public float range;
	public Transform positionReference;

	public HarvesterWeapon harvestRay;

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
		harvestRay.StartHarvesting(this, targetedHarvestable);
	}

	void StopHarvesting()
	{
		targetedHarvestable = null;
		harvestRay.StopHarvesting();
	}

	bool IsBetter(Harvestable a, Harvestable b)
	{
		return Vector2.SqrMagnitude (a.transform.position - transform.position) > 
			Vector2.SqrMagnitude (b.transform.position - transform.position);
	}

	void Harvest ()
	{
		var betterHarvestable = targetedHarvestable;

		foreach (var harvestable in harvestablesInRange) {
			if (IsBetter (betterHarvestable, harvestable)) {
				betterHarvestable = harvestable;
			}
		}

		if (betterHarvestable != targetedHarvestable) {
			StopHarvesting ();
			StartHarvesting (betterHarvestable);
		}

		if (harvestRay.IsDone () || !IsInRange (targetedHarvestable)) {
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
