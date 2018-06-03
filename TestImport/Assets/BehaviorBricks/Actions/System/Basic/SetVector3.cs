using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("Basic/SetVector3")]
    [Help("Sets a value to a Vector3 variable")]
    public class SetVector3 : BasePrimitiveAction
    {
        [OutParam("var")]
        [Help("output variable")]
        public Vector3 var;

        [InParam("value")]
        [Help("Value")]
        public Vector3 value;

        public override void OnStart()
        {
            var = value;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
