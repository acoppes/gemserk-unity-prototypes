using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    // ReSharper disable once UnusedMember.Global
    public class MovementSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<MovementComponent> movement;
            public ComponentDataArray<Position> position;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < m_Data.Length; i++)
            {
                var p = m_Data.position[i];
                var movement = m_Data.movement[i];

                var direction = new Vector2(movement.direction.x, movement.direction.y);
                direction.Normalize();
              
                p.Value.x += direction.x * movement.speed * dt;
                p.Value.y += direction.y * movement.speed * dt;

                m_Data.position[i] = p;

                movement.transform.position = p.Value;

                // resets movement direction after processing
                movement.direction = new float2(0, 0);
            }
        }
    }
}