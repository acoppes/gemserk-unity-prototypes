﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : IBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private readonly Func<TimeData, BehaviourTreeStatus> fn;

        public static string DebugCurrentNode;
        
        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            this.name = name;
            this.fn = fn;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            DebugCurrentNode = name;
            return fn(time);
        }
    }
}
