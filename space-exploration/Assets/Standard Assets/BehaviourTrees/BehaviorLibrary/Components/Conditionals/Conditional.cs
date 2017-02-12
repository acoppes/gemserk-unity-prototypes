using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Conditionals
{
    public class Conditional : BehaviorComponent
    {

		private ConditionalDelegate _Bool;

        /// <summary>
        /// Returns a return code equivalent to the test 
        /// -Returns Success if true
        /// -Returns Failure if false
        /// </summary>
        /// <param name="test">the value to be tested</param>
		public Conditional(ConditionalDelegate test)
        {
            _Bool = test;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {

            try
            {
                switch (_Bool())
                {
                    case true:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case false:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    default:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                }
            }
            catch (Exception e)
            {
				if (Debug.isDebugBuild) {
					Debug.Log(e.ToString());
				}
                ReturnCode = BehaviorReturnCode.Failure;
                return ReturnCode;
            }
        }
    }

	/// <summary>
	/// Should return an index that represents which of the behavior branches to perform
	/// </summary>
	public delegate bool ConditionalDelegate();
}
