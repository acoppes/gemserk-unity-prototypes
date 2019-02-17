using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace VirtualVillagers.Components
{
    public class ModelComponent : MonoBehaviour
    {
        [NonSerialized]
        public Model model;  
        public string modelAsset;
    }
    
    public class ModelSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentArray<ModelComponent> model;
            public ComponentArray<Transform> transform;
            public EntityArray entity;
        }
        
        [Inject] private Data _data;
        
        private readonly List<Model> _models = new List<Model>();

        protected override void OnUpdate()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                var model = _data.model[i];
                
                if (model.model == null)
                {
                    if (string.IsNullOrEmpty(model.modelAsset))
                    {
                        continue;
                    }

                    var modelPrefab = Resources.Load<GameObject>(model.modelAsset);
                    var gameObject = GameObject.Instantiate(modelPrefab);
                    model.model = gameObject.GetComponent<Model>();

                    model.model.entity = _data.entity[i];
                    _models.Add(model.model);
                }

                model.model.UpdateRender(_data.transform[i]);
//
//                if (!EntityManager.Exists(_data.entity[i]))
//                {
//                    GameObject.Destroy(model.model);
//                    model.model = null;
//                }
            }

            var toRemove = _models.Where(m => !EntityManager.Exists(m.entity)).ToList();
            toRemove.ForEach(m =>
            {
                GameObject.Destroy(m.gameObject);
                _models.Remove(m);
            });
            
//            _models.RemoveAll(m => !EntityManager.Exists(m.entity));
//            foreach (var model in _models)
//            {
//                if (!EntityManager.Exists(model.entity))
//                {
//                    // destroy or disable model
//                    GameObject.Destroy(model.gameObject);
//                    _models.Remove(model);
//                }
//            }
            
        }
    }
}