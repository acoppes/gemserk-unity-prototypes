using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;

namespace BBUnity.Actions
{

    [Action("Basic/SetBool")]
    [Help("Sets a value to a boolean variable")]
    public class SetBool : BasePrimitiveAction
    {
        [OutParam("var")]
        [Help("output variable")]
        public bool var;

        [InParam("value")]
        [Help("Value")]
        public bool value;

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
