using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

namespace Gemserk.BehaviourTree
{
    public class UnityBehaviourTreeContext : BehaviourTreeContext
    {
        private UnityEngine.GameObject _gameObject;

        private Unity.Entities.Entity _entity;

        private Unity.Entities.EntityManager _entityManager;

        private Dictionary<object, object> _managers = new Dictionary<object, object>();

        public void SetManager<T>(T t) where T : class
        {
            _managers.Add(typeof(T), t);
        }

        public T GetManager<T>() where T : class
        {
            return _managers[typeof(T)] as T;
        }

        public void SetObject(object o)
        {
            var go = o as GameObject;
            _gameObject = go;
            _entity = go.GetComponent<Unity.Entities.GameObjectEntity>().Entity;
            _entityManager = go.GetComponent<Unity.Entities.GameObjectEntity>().EntityManager;
        }

        public T GetComponent<T>() where T : UnityEngine.Component
        {
            // if (!_entityManager.Exists(_entity))
            //     throw new System.Exception("Entity should exist");
            return _entityManager.GetComponentObject<T>(_entity);
        }

        public T GetComponentData<T>() where T : struct, IComponentData
        {
            return _entityManager.GetComponentData<T>(_entity);
        }
    }
}