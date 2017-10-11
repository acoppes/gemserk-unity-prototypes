using UnityEngine;

public class HarvestSceneController : MonoBehaviour {

	public HarvestBehaviour harvester;
	public World world;

	void Awake () {
		harvester.world = world;
	}

}
