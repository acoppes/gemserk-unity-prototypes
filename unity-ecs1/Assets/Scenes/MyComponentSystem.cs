using UnityEngine;
using Unity.Entities;

namespace VirtualVillagers
{
    public class MyComponentSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentArray<MyComponent> myComponent;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (int i = 0; i < m_Data.Length; i++)
            {
                m_Data.myComponent[i].superValue++;
            }

            //foreach (var entity in GetEntities<Data>())
            //{
            //    var m = entity.myComponent;
            //    m.superValue++;
            //}
        }
    }
}
