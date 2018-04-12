using UnityEngine;
using Unity.Entities;
using Unity.Transforms2D;
using Unity.Transforms;

namespace VirtualVillagers
{
    public class MyComponentSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentArray<InputComponent> input;
            public ComponentArray<MovementComponent> movement;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (int i = 0; i < m_Data.Length; i++)
            {
                var horizontalValue = Input.GetAxis(m_Data.input[i].horizontalAxis);

                m_Data.movement[i].direction = new Unity.Mathematics.float2(horizontalValue, 0);
            }
        }
    }

    public class MovementSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentArray<MovementComponent> movement;
            public ComponentDataArray<Position2D> position;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (int i = 0; i < m_Data.Length; i++)
            {
                // m_Data.myComponent[i].superValue++;

                var p = m_Data.position[i];
                var movement = m_Data.movement[i];

                p.Value.x += movement.direction.x * movement.speed * dt;

                m_Data.position[i] = p;

                movement.transform.position = new Vector2(p.Value.x, p.Value.y);
            }
        }
    }
}
