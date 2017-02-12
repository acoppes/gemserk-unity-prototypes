using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Actions
{
    public class BehaviorAction : BehaviorComponent
    {
		private BehaviourActionDelegate _Action;

        public BehaviorAction() { }

		public BehaviorAction(BehaviourActionDelegate action)
        {
            _Action = action;
        }

        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (_Action())
                {
                    case BehaviorReturnCode.Success:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case BehaviorReturnCode.Failure:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    case BehaviorReturnCode.Running:
                        ReturnCode = BehaviorReturnCode.Running;
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

	public delegate BehaviorReturnCode BehaviourActionDelegate();
}


