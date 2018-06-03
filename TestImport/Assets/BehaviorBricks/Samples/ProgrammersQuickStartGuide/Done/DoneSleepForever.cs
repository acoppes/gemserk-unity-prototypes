using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

namespace BBSamples.PQSG // Programmers Quick Start Guide
{

    [Action("Samples/ProgQuickStartGuide/SleepForever")]
    [Help("Low-cost infinite action that never ends. It does not consume CPU at all.")]
    public class DoneSleepForever : BasePrimitiveAction
    {

        // Main class method, invoked by the execution engine.
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.SUSPENDED;
        } // OnUpdate

    } // class DoneSleepForever

} // namespace