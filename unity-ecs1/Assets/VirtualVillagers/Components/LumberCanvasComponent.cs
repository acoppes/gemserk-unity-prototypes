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
        public GameObject prefab;
        public Entity canvas;

        public int size;
        public Vector3 offset;
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


        protected override void OnUpdate()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                var lumberCanvas = _data.lumberCanvas[i];
//                var entity = _data.entities[i];

                if (EntityManager.Exists(lumberCanvas.canvas))
                    continue;

                var instanceObject = GameObject.Instantiate(lumberCanvas.prefab);
                var lumberUIComponent = instanceObject.GetComponent<LumberUIComponent>();

                lumberUIComponent.lumberEntity = lumberCanvas.GetComponent<GameObjectEntity>().Entity;

                lumberCanvas.canvas = instanceObject.GetComponent<GameObjectEntity>().Entity;
            }
        }
    }
}