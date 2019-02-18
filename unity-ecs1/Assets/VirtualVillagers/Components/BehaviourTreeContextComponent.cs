using System.Collections.Generic;
using UnityEngine;

namespace VirtualVillagers.Components
{
    public class SpawnerData
    {
        // for spawner behaviour
        // public string spawnItemsTag;
        public float spawnIdleTotalTime;
        public float spawnIdleCurrentTime;
        public int spawnItemsMax;
        public GameObject spawnPrefab;
    }

    public class FoodComsumerData
    {
        // for food consumer behaviour
        public GameObject foodSelection;
        public int foodConsumed;
    }

    public class WandererData
    {
        public float idleTotalTime;
        public float idleCurrentTime;
    }

    public class HarvesterData
    {
        public float harvestLumberMaxDistance;
        public float harvestLumberMinDistance;
        public GameObject harvestLumberCurrentTree;
    }

    // esto es claramente un dato de la instancia, la entidad misma, y al mismo tiempo
    // datos de juego, sea de manera abstracta o especifica

    // Podría estar en un componente, pero si no hay sistema que lo utilice, no tiene sentido, 
    // la otra podría ser crear sistemas para los comportamientos si tienen sentido para todas las entidades
    // pero si pudiera haber muchos harvesters distintos (lo cual me parece que tiene sentido) entonces como que 
    // vale la pena desacoplarlo de los sistemas y dejarlo del lado del behaviourtree/action/statescript, logica de gameplay

    public class BehaviourTreeContextComponent : MonoBehaviour
    {
        public enum ActionState
        {
            Idle,
            Moving,
            Harvesting
        }

        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public bool HasData(string name)
        {
            return _data.ContainsKey(name);
        }

        public T GetData<T>(string name)
        {
            return (T) _data[name];
        }

        public void SetData<T>(string name, T data) 
        {
            _data[name] = data;
        }

        public float spawnIdleTotalTime;
        public float spawnIdleCurrentTime;
        public int spawnItemsMax;
        public GameObject spawnPrefab;

        // for food consumer behaviour
        public GameObject foodSelection;
        public int foodConsumed;

        public float idleTotalTime;
        public float idleCurrentTime;

        //public float harvestLumberMaxDistance;
        //public float harvestLumberMinDistance;
        //public GameObject harvestLumberCurrentTree;

        public ActionState actionState;
    }
}