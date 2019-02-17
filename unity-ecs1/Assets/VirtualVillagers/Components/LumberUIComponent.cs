using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace VirtualVillagers.Components
{
    // this is the component for the ui itself
    public class LumberUIComponent : MonoBehaviour
    {
        public Entity lumberEntity;
        public float current;
        public float total;

        public bool visible;

        public int size;
    }
    
    public class LumberCanvasUISystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentArray<LumberUIComponent> lumberUI;
        }
        
        [Inject] private readonly Data _data;
        
        private readonly List<GameObject> _toDestroy = new List<GameObject>();

        protected override void OnUpdate()
        {
            // list to destroy
            
            for (var i = 0; i < _data.Length; i++)
            {
                var lumberUI = _data.lumberUI[i];

                if (!EntityManager.Exists(lumberUI.lumberEntity))
                {
                    _toDestroy.Add(lumberUI.gameObject);
                    continue;
                }

                if (!EntityManager.HasComponent<LumberComponent>(lumberUI.lumberEntity))
                {
                    _toDestroy.Add(lumberUI.gameObject);
                    continue;
                }
                
                if (!EntityManager.HasComponent<LumberCanvasComponent>(lumberUI.lumberEntity))
                {
                    _toDestroy.Add(lumberUI.gameObject);
                    continue;
                }

                var lumber = EntityManager.GetComponentObject<LumberComponent>(lumberUI.lumberEntity);
                
                var lumberTransform = EntityManager.GetComponentObject<Transform>(lumberUI.lumberEntity);
                var lumberCanvas = EntityManager.GetComponentObject<LumberCanvasComponent>(lumberUI.lumberEntity);
                
                // TODO: + offset
                lumberUI.transform.position = lumberTransform.position + lumberCanvas.offset;

                lumberUI.current = lumber.current;
                lumberUI.total = lumber.total;

                lumberUI.size = lumberCanvas.size;

                lumberUI.visible = true;
            }
            
            foreach (var go in _toDestroy)
            {
                GameObject.Destroy(go);
            }
            
            _toDestroy.Clear();
        }
    }
    
}