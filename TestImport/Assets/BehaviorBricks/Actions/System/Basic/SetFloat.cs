using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;

namespace BBUnity.Actions
{

    [Action("Basic/SetFloat")]
    [Help("Sets a value to a float variable")]
    public class SetFloat : BasePrimitiveAction
    {
        [OutParam("var")]
        [Help("output variable")]
        public float var;

        [InParam("value")]
        [Help("Value")]
        public float value;

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
