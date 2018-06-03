﻿using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus

namespace BBSamples.PQSG // Programmers Quick Start Guide
{

    [Action("Samples/ProgQuickStartGuide/Shoot")]
    [Help("Periodically clones a 'bullet' and shoots it throught the Forward axis " +
          "with the specified velocity. This action never ends.")]
    public class DoneShoot : DoneShootOnce
    {
        // Define the input parameter delay, with the waited game loops between shoots.
        [InParam("delay", DefaultValue = 30)]
        public int delay;

        // Game loops since the last shoot.
        private int elapsed = 0;

        // Main class method, invoked by the execution engine.
        public override TaskStatus OnUpdate()
        {
            if (delay > 0)
            {
                ++elapsed;
                elapsed %= delay;
                if (elapsed != 0)
                    return TaskStatus.RUNNING;
            }

            base.OnUpdate();
            return TaskStatus.RUNNING;

        } // OnUpdate

    } // class DoneShoot

} // namespace