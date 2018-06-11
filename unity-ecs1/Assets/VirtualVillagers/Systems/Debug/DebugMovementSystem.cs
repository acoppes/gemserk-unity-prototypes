using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    [UpdateAfter(typeof(MovementSystem))]
    public class DebugMovementSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public EntityArray entity;
            public ComponentArray<MovementComponent> movement;
            public ComponentArray<Transform> transform;
        }

        [Inject] private Data m_Data;

        private DebugMovementSystemBehaviour _debugBehaviour;
        
        protected override void OnUpdate()
        {
            if (_debugBehaviour == null)
            {
                var gameObject = new GameObject("~DebugMovementSystemObject");
                _debugBehaviour = gameObject.AddComponent<DebugMovementSystemBehaviour>();
            }

            _debugBehaviour.Reset();
            
            for (var i = 0; i < m_Data.Length; i++)
            {
                _debugBehaviour.AddDebug(m_Data.entity[i], m_Data.transform[i], m_Data.movement[i]);
            }
        }
    }
}