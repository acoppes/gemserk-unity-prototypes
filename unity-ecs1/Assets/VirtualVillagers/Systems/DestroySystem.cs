using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    [UpdateAfter(typeof(BehaviourTreeSystem))]
    public class DestroySystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<DestroyableComponent> destroyables;
        }

        [Inject] private Data m_Data;

        private readonly List<GameObject> _toDestroy = new List<GameObject>();
        
        protected override void OnUpdate()
        {
            for (var i = 0; i < m_Data.Length; i++)
            {
                if (m_Data.destroyables[i].shouldDestroy)
                {
                    _toDestroy.Add(m_Data.destroyables[i].gameObject);
                }
            }

            foreach (var destroyable in _toDestroy)
            {
                GameObject.Destroy(destroyable);
            }
            
            _toDestroy.Clear();
        }
    }
}