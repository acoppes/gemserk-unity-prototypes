using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace VirtualVillagers.Components
{
    // this is the component to say "I want to have a canvas"
    public class LumberCanvasComponent : MonoBehaviour
    {
        public Entity canvas;

        public int size;
        public Vector3 offset;

        public string modelAsset;
        
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + offset, 0.1f);
        }

#endif
    }

    // this is the component for the ui

    public class LumberCanvasSystem : ComponentSystem
    {
        private struct Data
        {
            public int Length;
            public ComponentArray<LumberCanvasComponent> lumberCanvas;
            public ComponentArray<LumberComponent> lumber;
//            public EntityArray entities;
        }
        
        [Inject] private Data _data;

        private Dictionary<string, GameObject> cachedPrefabs = new Dictionary<string, GameObject>();

        protected override void OnUpdate()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                var lumberCanvas = _data.lumberCanvas[i];
                
                if (EntityManager.Exists(lumberCanvas.canvas))
                    continue;

                if (string.IsNullOrEmpty(lumberCanvas.modelAsset))
                    continue;

                var lumberBarPrefab = Resources.Load<GameObject>(lumberCanvas.modelAsset);
                
                var instanceObject = GameObject.Instantiate(lumberBarPrefab);
                var lumberUIComponent = instanceObject.GetComponent<LumberUIComponent>();

                lumberUIComponent.lumberEntity = lumberCanvas.GetComponent<GameObjectEntity>().Entity;

                lumberCanvas.canvas = instanceObject.GetComponent<GameObjectEntity>().Entity;
            }
        }
    }
}