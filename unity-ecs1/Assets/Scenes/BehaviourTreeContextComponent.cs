using UnityEngine;

namespace VirtualVillagers
{
    public class BehaviourTreeContextComponent : MonoBehaviour
    {
        public string spawnItemsTag;
        public float spawnIdleTotalTime;
        public float spawnIdleCurrentTime;

        public GameObject foodSelection;
        public int foodConsumed;
    }
}