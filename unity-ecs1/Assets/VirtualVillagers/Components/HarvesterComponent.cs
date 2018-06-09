using Unity.Entities;
using UnityEngine;

namespace VirtualVillagers.Components
{
    public class HarvesterComponent : MonoBehaviour
    {
        public Entity currentLumberTarget;
        public Entity currentLumberMill;
        public float lumberPerSecond;
        public float maxLumber;
        public float currentLumber;
    }
}