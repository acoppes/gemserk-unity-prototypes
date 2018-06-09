using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class SpawnerSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<SpawnerComponent> spawners;
        }

        [Inject] private Data _data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;
            
            for (var i = 0; i < _data.Length; i++)
            {
                var spawner = _data.spawners[i];

                if (spawner.prefab == null)
                    continue;
                
                if (spawner.currentIdleTime + dt < spawner.totalIdleTime)
                {
                    spawner.currentIdleTime += dt;
                    continue;
                }
                                
                if (string.IsNullOrEmpty(spawner.prefab.tag))
                    continue;

                var spawnedEntities = GameObject.FindGameObjectsWithTag(spawner.prefab.tag);

                spawner._debugCurrentSpawned = spawnedEntities.Length;

                if (spawnedEntities.Length >= spawner.max)
                    continue;
                
                var x = UnityEngine.Random.Range(spawner.bounds.min.x, spawner.bounds.max.x);
                var y = UnityEngine.Random.Range(spawner.bounds.min.y, spawner.bounds.max.y);
                
                var spawnItem = GameObject.Instantiate(spawner.prefab);

                spawnItem.transform.position = new Vector3(x, y, 0);
                spawner.currentIdleTime = spawner.currentIdleTime + dt - spawner.totalIdleTime;
            }
           
        }
    }
}