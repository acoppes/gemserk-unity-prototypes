using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class HarvesterSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<Harvester> harvester;
        }
        
        [Inject] private Data _data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < _data.Length; i++)
            {
                // for each harvester with this lumber...
                var harvester = _data.harvester[i];

                if (!EntityManager.Exists(harvester.currentLumberTarget))
                    continue;

                if (!EntityManager.HasComponent<Lumber>(harvester.currentLumberTarget)) 
                    continue;
                
                var lumber = EntityManager.GetComponentObject<Lumber>(harvester.currentLumberTarget);

                var harvestedLumber = Mathf.Min(harvester.lumberPerSecond * dt, lumber.current);
                    
                lumber.current -= harvestedLumber;
                harvester.currentLumber += harvestedLumber;
                    
                lumber.harvesters++;

                harvester.currentLumberTarget = Entity.Null;
            }
           
        }
    }
}