using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBCore.Actions
{

    /// <summary>
    /// Implementation of the wait action using busy-waiting (spinning).
    /// </summary>
    [Action("Basic/WaitForSeconds")]
    [Help("Action that success after a period of time.")]
    public class WaitForSeconds : BasePrimitiveAction
    {
        [InParam("seconds", DefaultValue = 0.5f)]
        [Help("Amount of time to wait (in seconds)")]
        public float seconds;

        private float elapsedTime;

        public override void OnStart()
        {
            elapsedTime = 0;
        }

        public override TaskStatus OnUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= seconds)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }
    }
}
