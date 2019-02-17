using Unity.Entities;

namespace Gemserk.BehaviourTree
{
    public class UnityBehaviourTreeContext : BehaviourTreeContext
    {
        private UnityEngine.GameObject _gameObject;

        private Unity.Entities.Entity _entity;

        private Unity.Entities.EntityManager _entityManager;

        public void SetGameObject(UnityEngine.GameObject go)
        {
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