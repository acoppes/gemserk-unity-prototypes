using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;

namespace BBUnity.Actions
{

    [Action("Basic/SetInt")]
    [Help("Sets a value to an integer variable")]
    public class SetInt : BasePrimitiveAction
    {
        [OutParam("var")]
        [Help("output variable")]
        public int var;

        [InParam("value")]
        [Help("Value")]
        public int value;

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
