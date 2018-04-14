using Unity.Mathematics;
using UnityEngine;

namespace VirtualVillagers
{
    public class MovementComponent : MonoBehaviour
    {
        public float2 direction;
        public float speed = 1.0f;

        public bool hasDestination;
        public Vector2 destination;

        public float destinationDistance = 1.0f;

        public float idleTime;
        public float currentIdleTime;
    }
}
