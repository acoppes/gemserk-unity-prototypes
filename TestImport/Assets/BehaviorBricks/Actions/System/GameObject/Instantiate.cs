using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/Instantiate")]
    [Help("Clones the object original and returns the clone")]
    public class Instantiate : GOAction
    {
        [InParam("original")]
        [Help("Object to be cloned")]
        public GameObject original;

        [OutParam("instantiated")]
        [Help("Returned game object")]
        public GameObject instantiated;

        [InParam("position")]
        [Help("position for the clone")]
        public Vector3 position;

        public override void OnStart()
        {
            original = GameObject.Instantiate(original,position,original.transform.rotation) as GameObject;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
