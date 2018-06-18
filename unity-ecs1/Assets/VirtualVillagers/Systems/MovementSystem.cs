using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class MovementSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<MovementComponent> movement;
            public ComponentArray<Transform> transform;
        }

        [Inject] private Data m_Data;
        
        protected override void OnUpdate()
        {
//            var dt = Time.deltaTime;
            var simulation = SimulationTime.GetTime();
            
//            var _simulation = GetEntities<SimulationComponentData>()[0].simulationTime;
//
//            // GetEntities<SimulationData>();
//            
            if (simulation.frames == 0)
                return;
            
            var dt = simulation.dt;
            
            for (var i = 0; i < m_Data.Length; i++)
            {
//                var p = m_Data.position[i];
                var p = m_Data.transform[i].position;
                var movement = m_Data.movement[i];

                var direction = new Vector2(movement.direction.x, movement.direction.y);
                direction.Normalize();

                movement.velocity = direction * movement.speed * dt;
              
                p.x += movement.velocity.x;
                p.y += movement.velocity.y;

                m_Data.transform[i].position = p;

                // movement.transform.position = p.Value;

                // resets movement direction after processing
                movement.direction = new float2(0, 0);
            }
        }
    }
}