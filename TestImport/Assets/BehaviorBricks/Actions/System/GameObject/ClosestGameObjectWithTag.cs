using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/ClosestGameObjectWithTag")]
    [Help("Finds the closest game object with a given tag")]
    public class ClosestGameObjectWithTag : GOAction
    {
        [InParam("tag")]
        [Help("Tag of the target game object")]
        public string tag;

        [OutParam("foundGameObject")]
        [Help("The closest game object with the given tag")]
        public GameObject foundGameObject;

        private float elapsedTime;

        public override void OnStart()
        {
            float dist = float.MaxValue;
            foreach(GameObject go in GameObject.FindGameObjectsWithTag(tag))
            {
                float newdist = (go.transform.position + gameObject.transform.position).sqrMagnitude;
                if(newdist < dist)
                {
                    dist = newdist;
                    foundGameObject = go;
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
