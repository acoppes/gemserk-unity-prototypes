using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("Navigation/MoveToRandomPosition")]
    [Help("Gets a random position from a given area and moves the game object to that point by using a NavMeshAgent")]
    public class MoveToRandomPosition : GOAction
    {
        private UnityEngine.AI.NavMeshAgent navAgent;

        [InParam("area")]
        [Help("game object that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted")]
        public GameObject area;

        public override void OnStart()
        {
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
            navAgent.SetDestination(getRandomPosition());

            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = false;
            #else
                navAgent.Resume();
            #endif
        }

        public override TaskStatus OnUpdate()
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }

        private Vector3 getRandomPosition()
        {
            BoxCollider boxCollider = area != null ? area.GetComponent<BoxCollider>() : null;
            if (boxCollider != null)
            {
                return new Vector3(UnityEngine.Random.Range(area.transform.position.x - area.transform.localScale.x * boxCollider.size.x * 0.5f,
                                                            area.transform.position.x + area.transform.localScale.x * boxCollider.size.x * 0.5f),
                                   area.transform.position.y,
                                   UnityEngine.Random.Range(area.transform.position.z - area.transform.localScale.z * boxCollider.size.z * 0.5f,
                                                            area.transform.position.z + area.transform.localScale.z * boxCollider.size.z * 0.5f));
            }
            else
            {
                SphereCollider sphereCollider = area != null ? area.GetComponent<SphereCollider>() : null;
                if (sphereCollider != null)
                {
                    float distance = UnityEngine.Random.Range(-sphereCollider.radius, area.transform.localScale.x * sphereCollider.radius);
                    float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
                    return new Vector3(area.transform.position.x + distance * Mathf.Cos(angle),
                                       area.transform.position.y,
                                       area.transform.position.z + distance * Mathf.Sin(angle));
                }
                else
                {
                    return gameObject.transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, UnityEngine.Random.Range(-5f, 5f));
                }
            }
        }

        public override void OnAbort()
        {
            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = true;
            #else
                navAgent.Stop();
            #endif
        }
    }
}
