using UnityEngine;

public class HarvestSceneController : MonoBehaviour {

	public HarvestBehaviour harvester;
	public World world;

	void Awake () {
		harvester.world = world;

		var harvestables = FindObjectsOfType<Harvestable> ();
		foreach (var harvestable in harvestables) {
			harvestable.world = world;
		}
	}

}
