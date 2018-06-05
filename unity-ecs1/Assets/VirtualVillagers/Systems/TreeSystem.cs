using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class TreeSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<LumberHolder> lumbers;
            public ComponentArray<Components.Tree> trees;
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
                    lumber.current += _data.trees[i].regenerationPerSecond * dt;
                    lumber.current = Mathf.Min(lumber.current, lumber.total);
                }

                // resets harvesters count
                lumber.harvesters = 0;
            }
           
        }
    }
}