using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Composites;
using UnityEngine;

namespace BehaviorLibrary
{
    public enum BehaviorReturnCode
    {
        Failure,
        Success,
        Running
    }

    public delegate BehaviorReturnCode BehaviorReturn();

    /// <summary>
    /// 
    /// </summary>
    public class Behavior
    {

        private RootSelector _Root;

        private BehaviorReturnCode _ReturnCode;

        public BehaviorReturnCode ReturnCode
        {
            get { return _ReturnCode; }
            set { _ReturnCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        public Behavior(RootSelector root)
        {
            _Root = root;
        }

        /// <summary>
        /// perform the behavior
        /// </summary>
        public BehaviorReturnCode Behave()
        {
            try
            {
                switch (_Root.Behave())
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
#if DEBUG
                Console.Error.WriteLine(e.ToString());
#endif
                ReturnCode = BehaviorReturnCode.Failure;
                return ReturnCode;
            }
        }
    }
}
