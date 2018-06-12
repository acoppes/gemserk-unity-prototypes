using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace VirtualVillagers.Components
{
    // this is the component to say "I want to have a canvas"
    public class LumberCanvasComponent : MonoBehaviour
    {
        // offset
        // custom prefab (or archetype)
        
        // current instance (if not created already)
        public Entity canvas;

        public int size;
        public Vector3 offset;
        
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

        private GameObject _lumberBarPrefab;

        public void SetLumberBarPrefab(GameObject lumberBarPrefab)
        {
            _lumberBarPrefab = lumberBarPrefab;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                var lumberCanvas = _data.lumberCanvas[i];
//                var entity = _data.entities[i];

                if (EntityManager.Exists(lumberCanvas.canvas))
                    continue;

                var instanceObject = GameObject.Instantiate(_lumberBarPrefab);
                var lumberUIComponent = instanceObject.GetComponent<LumberUIComponent>();

                lumberUIComponent.lumberEntity = lumberCanvas.GetComponent<GameObjectEntity>().Entity;

                lumberCanvas.canvas = instanceObject.GetComponent<GameObjectEntity>().Entity;
            }
        }
    }
}