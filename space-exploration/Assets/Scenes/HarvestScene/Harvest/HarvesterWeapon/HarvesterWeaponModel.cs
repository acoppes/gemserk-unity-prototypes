using UnityEngine;

public abstract class HarvesterWeaponModel : MonoBehaviour
{
	public abstract void StartHarvesting(HarvestBehaviour harvester, Harvestable harvestable);

	public abstract void StopHarvesting();
}
