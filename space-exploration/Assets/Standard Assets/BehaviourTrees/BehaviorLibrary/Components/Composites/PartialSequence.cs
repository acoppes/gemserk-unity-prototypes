using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class PartialSequence : BehaviorComponent
    {

        protected BehaviorComponent[] _Behaviors;

        private short _sequence = 0;

        private short _seqLength = 0;
        
        /// <summary>
        /// Performs the given behavior components sequentially (one evaluation per Behave call)
        /// Performs an AND-Like behavior and will perform each successive component
        /// -Returns Success if all behavior components return Success
        /// -Returns Running if an individual behavior component returns Success or Running
        /// -Returns Failure if a behavior components returns Failure or an error is encountered
        /// </summary>
        /// <param name="behaviors">one to many behavior components</param>
        public PartialSequence(params BehaviorComponent[] behaviors)
        {
            _Behaviors = behaviors;
            _seqLength = (short) _Behaviors.Length;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            //while you can go through them, do so
            while (_sequence < _seqLength)
            {
                try
                {
                    switch (_Behaviors[_sequence].Behave())
                    {
                        case BehaviorReturnCode.Failure:
                            _sequence = 0;
                            ReturnCode = BehaviorReturnCode.Failure;
                            return ReturnCode;
                        case BehaviorReturnCode.Success:
                            _sequence++;
                            ReturnCode = BehaviorReturnCode.Running;
                            return ReturnCode;
                        case BehaviorReturnCode.Running:
                            ReturnCode = BehaviorReturnCode.Running;
                            return ReturnCode;
                    }
                }
                catch (Exception e)
                {
	
					if (Debug.isDebugBuild) {
						Console.Error.WriteLine(e.ToString());
					}

                    _sequence = 0;
                    ReturnCode = BehaviorReturnCode.Failure;
                    return ReturnCode;
                }

            }

            _sequence = 0;
            ReturnCode = BehaviorReturnCode.Success;
            return ReturnCode;

        }

    }
}
