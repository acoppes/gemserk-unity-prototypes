using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;

namespace BBUnity.Actions
{

    [Action("Basic/ToggleBool")]
    [Help("Toggle a boolean variable")]
    public class ToggleBool : BasePrimitiveAction
    {
        [OutParam("var")]
        [Help("output variable")]
        public bool var;

        public override void OnStart()
        {
            var = !var;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
