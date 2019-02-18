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

                    // TODO: pool models

                    var modelPrefab = Resources.Load<GameObject>(model.modelAsset);
                    var gameObject = GameObject.Instantiate(modelPrefab);
                    model.model = gameObject.GetComponent<Model>();

                    model.model.entity = _data.entity[i];
                    _models.Add(model.model);
                }

                model.model.UpdateRender(_data.transform[i]);

                if (this.EntityManager.HasComponent<BehaviourTreeContextComponent>(_data.entity[i])) {
                    var btContext = this.EntityManager.GetComponentObject<BehaviourTreeContextComponent>(_data.entity[i]);
                    // var speed = Vector2.SqrMagnitude(movement.velocity);

                    var animator = model.model.GetComponentInChildren<Animator>();
                    if (animator != null)
                    {
                        animator.SetBool("Moving", btContext.actionState == BehaviourTreeContextComponent.ActionState.Moving);
                        animator.SetBool("Harvesting", btContext.actionState == BehaviourTreeContextComponent.ActionState.Harvesting);
                    }

                    // Me imagino que necesito una especie de "estado" del unit en algun lado
                    // y traducir este estado al animator.
                }
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