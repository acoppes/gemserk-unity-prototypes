using UnityEngine;
using Unity.Entities;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    // ReSharper disable once UnusedMember.Global
    public class PlayerInputSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentArray<InputComponent> input;
            public ComponentArray<MovementComponent> movement;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < m_Data.Length; i++)
            {
                var horizontalValue = Input.GetAxis(m_Data.input[i].horizontalAxis);

                m_Data.movement[i].direction = new Unity.Mathematics.float2(horizontalValue, 0);
            }
        }
    }
}
