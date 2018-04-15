using Unity.Mathematics;
using UnityEngine;

namespace VirtualVillagers
{
    public class MovementComponent : MonoBehaviour
    {
        public float2 direction;
        public float speed = 1.0f;

        public float destinationDistance = 1.0f;
        
        // for wander behaviour
        public bool hasDestination;
        public Vector2 destination;
    }
}
