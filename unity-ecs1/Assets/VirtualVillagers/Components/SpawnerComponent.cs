using UnityEngine;

namespace VirtualVillagers.Components
{
    public class SpawnerComponent : MonoBehaviour
    {
        public float totalIdleTime;
        public float currentIdleTime;
        public float max;
        public Bounds bounds;
        public GameObject prefab;

        public int _debugCurrentSpawned;
    }
}