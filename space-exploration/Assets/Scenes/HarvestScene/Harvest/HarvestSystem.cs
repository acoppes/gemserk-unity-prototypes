using UnityEngine;
using System;
using System.Collections.Generic;

public class HarvestSystem : MonoBehaviour
{
	readonly List<Harvestable> harvestables = new List<Harvestable>();
	readonly List<HarvestBehaviour> harvesters = new List<HarvestBehaviour>();

	public void Register(HarvestBehaviour harvester) 
	{
		harvesters.Add (harvester);
	}

	public void Unregister(HarvestBehaviour harvester)
	{
		harvesters.Remove (harvester);
	}

	void FixedUpdate()
	{
		// foreach harvestable
		foreach (var harvester in harvesters) {
			harvester.ProcessHarvestables (harvestables);
		}
	}
}
