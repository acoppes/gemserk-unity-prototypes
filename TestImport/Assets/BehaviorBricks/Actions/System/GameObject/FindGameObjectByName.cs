using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/FindByName")]
    [Help("Finds a game object by name")]
    public class FindGameObjectByName : BasePrimitiveAction
    {
        [InParam("name")]
        [Help("Name of the target game object")]
        public string name;

        [OutParam("foundGameObject")]
        [Help("Found game object")]
        public GameObject foundGameObject;

        private float elapsedTime;

        public override void OnStart()
        {
            foundGameObject = GameObject.Find(name);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
