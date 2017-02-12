using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class PartialSelector : BehaviorComponent
    {

        protected BehaviorComponent[] _Behaviors;

        private short _selections = 0;

        private short _selLength = 0;

        /// <summary>
		/// Selects among the given behavior components (one evaluation per Behave call)
        /// Performs an OR-Like behavior and will "fail-over" to each successive component until Success is reached or Failure is certain
        /// -Returns Success if a behavior component returns Success
        /// -Returns Running if a behavior component returns Failure or Running
        /// -Returns Failure if all behavior components returned Failure or an error has occured
        /// </summary>
        /// <param name="behaviors">one to many behavior components</param>
        public PartialSelector(params BehaviorComponent[] behaviors)
        {
            _Behaviors = behaviors;
            _selLength = (short)_Behaviors.Length;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            while (_selections < _selLength)
            {
                try
                {
                    switch (_Behaviors[_selections].Behave())
                    {
                        case BehaviorReturnCode.Failure:
                            _selections++;
                            ReturnCode = BehaviorReturnCode.Running;
                            return ReturnCode;
                        case BehaviorReturnCode.Success:
                            _selections = 0;
                            ReturnCode = BehaviorReturnCode.Success;
                            return ReturnCode;
                        case BehaviorReturnCode.Running:
                            ReturnCode = BehaviorReturnCode.Running;
                            return ReturnCode;
                        default:
                            _selections++;
                            ReturnCode = BehaviorReturnCode.Failure;
                            return ReturnCode;
                    }
                }
                catch (Exception e)
                {
					if (Debug.isDebugBuild) {
						Debug.Log(e.ToString());
					}

                    _selections++;
                    ReturnCode = BehaviorReturnCode.Failure;
                    return ReturnCode;
                }
            }

            _selections = 0;
            ReturnCode = BehaviorReturnCode.Failure;
            return ReturnCode;
        }


    }
}
