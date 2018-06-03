using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/FindByTag")]
    [Help("Finds a game object by name")]
    public class FindGameObjectByTag : BasePrimitiveAction
    {
        [InParam("tag")]
        [Help("Tag of the target game object")]
        public string tag;

        [OutParam("foundGameObject")]
        [Help("Found game object")]
        public GameObject foundGameObject;

        private float elapsedTime;

        public override void OnStart()
        {
            foundGameObject = GameObject.FindWithTag(tag);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
