using Unity.Mathematics;
using UnityEngine;

namespace VirtualVillagers.Components
{
    public class MovementComponent : MonoBehaviour
    {
        public float2 direction;
        public float speed = 1.0f;

        public float destinationDistance = 1.0f;
        
        // for wander behaviour
        public bool hasDestination;
        public Vector2 destination;

        public void SetDestination(Vector2 destination)
        {
            this.destination = destination;
            this.hasDestination = true;
        }
    }
}
