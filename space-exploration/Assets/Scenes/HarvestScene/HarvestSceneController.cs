using UnityEngine;

public class HarvestSceneController : MonoBehaviour {

	public HarvestBehaviour harvester;
	public Harvestable harvestable;
	public World world;

	void Awake () {
		harvester.world = world;
		harvestable.world = world;
	}

}
