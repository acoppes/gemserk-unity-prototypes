using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class DebugMovementSystemBehaviour : MonoBehaviour
    {
        private struct MovementDebug
        {
            public Entity entity;
            public Vector2 position;
            public Vector2 velocity;
        }
        
        private readonly MovementDebug[] _movementDebugs = new MovementDebug[100];

        private int _current;
        
        public void Reset()
        {
            _current = 0;
        }

        private void OnDrawGizmos()
        {
            for (var i = 0; i < _current; i++)
            {
                var movementDebug = _movementDebugs[i];
                Gizmos.DrawLine(movementDebug.position, movementDebug.position + movementDebug.velocity * 30);
            }
        }

        public void AddDebug(Entity entity, Transform transform, MovementComponent movement)
        {
            if (_current >= _movementDebugs.Length)
                return;
            
            _movementDebugs[_current++] = new MovementDebug
            {
                entity = entity,
                position = transform.position,
                velocity = movement.velocity
            };
        }
    }
}