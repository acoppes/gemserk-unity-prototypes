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
            public ComponentArray<LumberComponent> lumbers;
            public ComponentArray<Components.TreeComponent> trees;
        }

        [Inject] private Data _data;
        
        protected override void OnUpdate()
        {
            var dt = Time.deltaTime;
            
            for (var i = 0; i < _data.Length; i++)
            {
                // for each harvester with this lumber...
                var lumber = _data.lumbers[i];
                var tree = _data.trees[i];
                
                // lumber can only be regenerated while no harvesters harvesting
                if (lumber.harvesters == 0)
                {
                    lumber.current += _data.trees[i].regenerationPerSecond * dt;
                    lumber.current = Mathf.Min(lumber.current, lumber.total);
                    
                    // if tree at maximum
                    // then grow
                    if (tree.currentSize < tree.maxSize && lumber.current >= lumber.total)
                    {
                        tree.currentSize++;
                        lumber.total = lumber.total + tree.lumberPerSize;
                    }
                }

                // resets harvesters count
                lumber.harvesters = 0;

                if (tree.seeds > 0 && tree.currentSize == tree.maxSize && lumber.current >= lumber.total)
                { 
                    var spawnedTreeObject = GameObject.Instantiate(tree.seedPrefab);
                    
                    var randomPosition = Random.insideUnitCircle * 
                                         Random.Range(tree.minSpawnDistance, tree.maxSpawnDistance);
						
                    spawnedTreeObject.transform.position = tree.transform.position 
                                                    + (Vector3) randomPosition;

                    var spawnedTree = spawnedTreeObject.GetComponent<Components.TreeComponent>();
                    spawnedTree.currentSize = 1;
                    spawnedTree.seedPrefab = tree.seedPrefab; // copy here to avoid referencing to itself.

                    var spawnedTreeLumber = spawnedTreeObject.GetComponent<LumberComponent>();
                    spawnedTreeLumber.total = spawnedTree.lumberPerSize;
                    spawnedTreeLumber.current = spawnedTreeLumber.total;
                    
                    tree.seeds--;
                }
            }
           
        }
    }
}