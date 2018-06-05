using UnityEngine;

namespace VirtualVillagers.Components
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

        public int treeCurrentSize;
        public int treeMaxSize = 3;
        public float treeMinSpawnDistance = 1.0f;
        public float treeMaxSpawnDistance = 3.0f;
        public int treeSeeds = 1;
        public float treeLumberPerSize = 5.0f;

        public float harvestLumberTotal = 10.0f;
        public float harvestLumberSpeed = 1.0f;
        public float harvestLumberMaxDistance = 10.0f;
        public float harvestLumberMinDistance = 1.5f;
        public GameObject harvestLumberCurrentTree = null;

        public float lumberMillLumberCurrent = 0.0f;
    }
}