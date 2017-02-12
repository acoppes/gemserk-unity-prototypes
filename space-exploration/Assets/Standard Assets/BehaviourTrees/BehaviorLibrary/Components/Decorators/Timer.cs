using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    public class Timer : BehaviorComponent
    {

		private TimerDelegate _ElapsedTimeFunction;

        private BehaviorComponent _Behavior;

        private int _TimeElapsed = 0;

        private int _WaitTime;

        /// <summary>
        /// executes the behavior after a given amount of time in miliseconds has passed
        /// </summary>
        /// <param name="elapsedTimeFunction">function that returns elapsed time</param>
        /// <param name="timeToWait">maximum time to wait before executing behavior</param>
        /// <param name="behavior">behavior to run</param>
		public Timer(TimerDelegate elapsedTimeFunction, int timeToWait, BehaviorComponent behavior)
        {
            _ElapsedTimeFunction = elapsedTimeFunction;
            _Behavior = behavior;
            _WaitTime = timeToWait;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                _TimeElapsed += _ElapsedTimeFunction();

                if (_TimeElapsed >= _WaitTime)
                {
                    _TimeElapsed = 0;
                    ReturnCode = _Behavior.Behave();
                    return ReturnCode;
                }
                else
                {
                    ReturnCode = BehaviorReturnCode.Running;
                    return BehaviorReturnCode.Running;
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

	/// <summary>
	/// Should return an int which represents num milliseconds 
	/// </summary>
	public delegate int TimerDelegate();
}
