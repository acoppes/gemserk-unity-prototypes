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
            public ComponentArray<HarvesterComponent> harvester;
        }
        
        [Inject] private Data _data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < _data.Length; i++)
            {
                // for each harvester with this lumber...
                var harvester = _data.harvester[i];

                if (EntityManager.Exists(harvester.currentLumberTarget))
                {
                    if (EntityManager.HasComponent<LumberComponent>(harvester.currentLumberTarget))
                    {
                        var lumberHolder = EntityManager.GetComponentObject<LumberComponent>(harvester.currentLumberTarget);

                        var harvestedLumber = Mathf.Min(harvester.lumberPerSecond * dt, lumberHolder.current);
                        harvestedLumber = Mathf.Min(harvestedLumber, harvester.maxLumber - harvester.currentLumber);

                        // consider harvester total too?

                        lumberHolder.current -= harvestedLumber;
                        harvester.currentLumber += harvestedLumber;

                        lumberHolder.harvesters++;

                        harvester.currentLumberTarget = Entity.Null;
                    }
                }

                if (EntityManager.Exists(harvester.currentLumberMill))
                {
                    if (EntityManager.HasComponent<LumberMillComponent>(harvester.currentLumberMill))
                    {
                        var lumberHolder = EntityManager.GetComponentObject<LumberComponent>(harvester.currentLumberMill);
                        
                        var returnedLumber = Mathf.Min(harvester.lumberPerSecond * dt, harvester.currentLumber);
                        returnedLumber = Mathf.Min(returnedLumber, lumberHolder.total - lumberHolder.current);
                        
                        lumberHolder.current += returnedLumber;
                        harvester.currentLumber -= returnedLumber;

//                        if (lumberHolder.total < lumberHolder.current)
//                            lumberHolder.total = lumberHolder.current;

                        harvester.currentLumberMill = Entity.Null;
                    }
                }
            }
           
        }
    }
}