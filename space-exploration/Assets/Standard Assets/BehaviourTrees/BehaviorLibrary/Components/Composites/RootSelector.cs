using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class RootSelector : PartialSelector
    {

        private BehaviorComponent[] _Behaviors;

        private Func<int> _Index;

        /// <summary>
        /// The selector for the root node of the behavior tree
        /// </summary>
        /// <param name="index">an index representing which of the behavior branches to perform</param>
        /// <param name="behaviors">the behavior branches to be selected from</param>
        public RootSelector(Func<int> index, params BehaviorComponent[] behaviors)
        {
            _Index = index;
            _Behaviors = behaviors;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (_Behaviors[_Index.Invoke()].Behave())
                {
                    case BehaviorReturnCode.Failure:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    case BehaviorReturnCode.Success:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case BehaviorReturnCode.Running:
                        ReturnCode = BehaviorReturnCode.Running;
                        return ReturnCode;
                    default:
                        ReturnCode = BehaviorReturnCode.Running;
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
	public delegate int SelectorIndexDelegate();

}
