using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    public class Counter : BehaviorComponent
    {
        private int _MaxCount;
        private int _Counter = 0;

        private BehaviorComponent _Behavior;

        /// <summary>
        /// executes the behavior based on a counter
        /// -each time Counter is called the counter increments by 1
        /// -Counter executes the behavior when it reaches the supplied maxCount
        /// </summary>
        /// <param name="maxCount">max number to count to</param>
        /// <param name="behavior">behavior to run</param>
        public Counter(int maxCount, BehaviorComponent behavior)
        {
            _MaxCount = maxCount;
            _Behavior = behavior;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                if (_Counter < _MaxCount)
                {
                    _Counter++;
                    ReturnCode = BehaviorReturnCode.Running;
                    return BehaviorReturnCode.Running;
                }
                else
                {
                    _Counter = 0;
                    ReturnCode = _Behavior.Behave();
                    return ReturnCode;
                }
            }
            catch (Exception e)
            {
				if (Debug.isDebugBuild) {
					Debug.Log(e.ToString());
				}
                ReturnCode = BehaviorReturnCode.Failure;
                return BehaviorReturnCode.Failure;
            }
        }
    }
}
