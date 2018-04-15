using UnityEngine;

namespace VirtualVillagers
{
    public class BehaviourTreeContextComponent : MonoBehaviour
    {
        // for spawner behaviour
        public string spawnItemsTag;
        public float spawnIdleTotalTime;
        public float spawnIdleCurrentTime;

        // for food consumer behaviour
        public GameObject foodSelection;
        public int foodConsumed;

        // for wander behaviour
        public bool hasWanderDestination;
        public Vector2 wanderDestination;

        // for idle behaviour
        public float idleTotalTime;
        public float idleCurrentTime;
    }
}