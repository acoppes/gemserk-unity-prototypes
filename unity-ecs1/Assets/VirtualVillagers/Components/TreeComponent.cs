using UnityEngine;

namespace VirtualVillagers.Components
{
    public class TreeComponent : MonoBehaviour
    {
        public int currentSize;
        public int maxSize = 3;
        
        public float minSpawnDistance = 1.0f;
        public float maxSpawnDistance = 3.0f;
        
        public GameObject seedPrefab;

        public int seeds = 1;
        public float lumberPerSize = 30.0f;
        
        public float regenerationPerSecond = 0.5f;

    }
}