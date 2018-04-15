using UnityEngine;

namespace VirtualVillagers
{
    public class BehaviourTreeContextComponent : MonoBehaviour
    {
        // for spawner behaviour
        // public string spawnItemsTag;
        public float spawnIdleTotalTime;
        public float spawnIdleCurrentTime;
        public int spawnItemsMax;
        public GameObject spawnPrefab;

        // for food consumer behaviour
        public GameObject foodSelection;
        public int foodConsumed;
        
        // for idle behaviour
        public float idleTotalTime;
        public float idleCurrentTime;

        public float treeGrowTime;
        public int currentTreeSize;
        public int maxTreeSize;
    }
}