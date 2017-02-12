using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    public class RandomDecorator : BehaviorComponent
    {

        private float _Probability;

        private Func<float> _RandomFunction;

        private BehaviorComponent _Behavior;

        /// <summary>
        /// randomly executes the behavior
        /// </summary>
        /// <param name="probability">probability of execution</param>
        /// <param name="randomFunction">function that determines probability to execute</param>
        /// <param name="behavior">behavior to execute</param>
        public RandomDecorator(float probability, Func<float> randomFunction, BehaviorComponent behavior)
        {
            _Probability = probability;
            _RandomFunction = randomFunction;
            _Behavior = behavior;
        }


        public override BehaviorReturnCode Behave()
        {
            try
            {
                if (_RandomFunction() <= _Probability)
                {
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
	/// Should return a float that represents a probability
	/// </summary>
	public delegate float RandomDelegate();
}
