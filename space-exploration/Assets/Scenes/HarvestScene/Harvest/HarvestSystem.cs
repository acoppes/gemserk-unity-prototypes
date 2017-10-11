using UnityEngine;
using System;
using System.Collections.Generic;

public class HarvestSystem : MonoBehaviour
{
	readonly List<Harvestable> harvestables = new List<Harvestable>();
	readonly List<HarvestBehaviour> harvesters = new List<HarvestBehaviour>();

	readonly List<Harvestable> activeHarvestables = new List<Harvestable>();

	public void Register(HarvestBehaviour harvester) 
	{
		harvesters.Add (harvester);
	}

	public void Unregister(HarvestBehaviour harvester)
	{
		harvesters.Remove (harvester);
	}

	public void Register(Harvestable harvestable) 
	{
		harvestables.Add (harvestable);
	}

	public void Unregister(Harvestable harvestable)
	{
		harvestables.Remove (harvestable);
	}

	void FixedUpdate()
	{
		// foreach harvestable
		activeHarvestables.Clear();

		foreach (var harvestable in harvestables) {
			if (harvestable.IsDepleted () || !harvestable.isActiveAndEnabled)
				continue;
			activeHarvestables.Add (harvestable);
		}

		foreach (var harvester in harvesters) {
			harvester.ProcessHarvestables (activeHarvestables);
		}
	}
}
