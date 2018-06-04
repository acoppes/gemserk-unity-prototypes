using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class LumberSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<Lumber> lumbers;
        }

        [Inject] private Data m_Data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < m_Data.Length; i++)
            {
                // for each harvester with this lumber...
                var lumber = m_Data.lumbers[i];

                lumber.current += lumber.regenerationPerSecond * dt;
                lumber.current = Mathf.Min(lumber.current, lumber.total);

                // resets harvesters count
                lumber.harvesters = 0;
            }
           
        }
    }
}