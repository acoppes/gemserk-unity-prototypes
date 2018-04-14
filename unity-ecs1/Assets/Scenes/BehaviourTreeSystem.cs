using Unity.Entities;
using UnityEngine;

namespace VirtualVillagers
{
    // ReSharper disable once UnusedMember.Global
    public class BehaviourTreeSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<BehaviourTreeComponent> behaviourTree;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < m_Data.Length; i++)
            {
                // m_Data.myComponent[i].superValue++;

                var bt = m_Data.behaviourTree[i];
                bt.Process(dt);
            }
        }
    }
}