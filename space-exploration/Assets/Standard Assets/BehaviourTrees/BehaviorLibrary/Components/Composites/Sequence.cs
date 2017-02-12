using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class Sequence : BehaviorComponent
    {

        private BehaviorComponent[] _behaviors;

        /// <summary>
        /// attempts to run the behaviors all in one cycle
        /// -Returns Success when all are successful
        /// -Returns Failure if one behavior fails or an error occurs
        /// -Returns Running if any are running
        /// </summary>
        /// <param name="behaviors"></param>
        public Sequence(params BehaviorComponent[] behaviors)
        {
            _behaviors = behaviors;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
			//add watch for any running behaviors
			bool anyRunning = false;

            for(int i = 0; i < _behaviors.Length;i++)
            {
                try
                {
                    switch (_behaviors[i].Behave())
                    {
                        case BehaviorReturnCode.Failure:
                            ReturnCode = BehaviorReturnCode.Failure;
                            return ReturnCode;
                        case BehaviorReturnCode.Success:
                            continue;
                        case BehaviorReturnCode.Running:
							anyRunning = true;
                            continue;
                        default:
                            ReturnCode = BehaviorReturnCode.Success;
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

			//if none running, return success, otherwise return running
            ReturnCode = !anyRunning ? BehaviorReturnCode.Success : BehaviorReturnCode.Running;
            return ReturnCode;
        }


    }
}
