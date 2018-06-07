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
        
//        public struct SimulationData
//        {
////            public int Length;
//            public SimulationTime simulationTime;
//        }

        [Inject] private Data m_Data;
        // [Inject] private SimulationData _simulationData;
        
        protected override void OnUpdate()
        {
//            var dt = Time.deltaTime;
            var _simulation = GetEntities<SimulationComponentData>()[0].simulationTime;

            // GetEntities<SimulationData>();
            
            if (_simulation.frames == 0)
                return;
            
            var dt = _simulation.dt;
            
            for (var i = 0; i < m_Data.Length; i++)
            {
//                var p = m_Data.position[i];
                var p = m_Data.transform[i].position;
                var movement = m_Data.movement[i];

                var direction = new Vector2(movement.direction.x, movement.direction.y);
                direction.Normalize();
              
                p.x += direction.x * movement.speed * dt;
                p.y += direction.y * movement.speed * dt;

                m_Data.transform[i].position = p;

                // movement.transform.position = p.Value;

                // resets movement direction after processing
                movement.direction = new float2(0, 0);
            }
        }
    }
}