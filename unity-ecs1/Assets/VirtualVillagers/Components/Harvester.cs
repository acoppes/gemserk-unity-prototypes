using Unity.Entities;
using UnityEngine;

namespace VirtualVillagers.Components
{
    public class Harvester : MonoBehaviour
    {
        public Entity currentLumberTarget;
        public float lumberPerSecond;
        public float maxLumber;
        public float currentLumber;
    }
}