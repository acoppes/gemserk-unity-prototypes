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

        [Inject] private Data _data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < _data.Length; i++)
            {
                // for each harvester with this lumber...
                var lumber = _data.lumbers[i];

                // lumber can only be regenerated while no harvesters harvesting
                if (lumber.harvesters == 0)
                {
                    lumber.current += lumber.regenerationPerSecond * dt;
                    lumber.current = Mathf.Min(lumber.current, lumber.total);
                }

                // resets harvesters count
                lumber.harvesters = 0;
            }
           
        }
    }
}