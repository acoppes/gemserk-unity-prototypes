using System;

namespace FluentBehaviourTree
{
    public class BaseConditionCallbackNode : IBehaviourTreeNode
    {
        private readonly Func<object, TimeData, bool> _fn;
        
        public BaseConditionCallbackNode(Func<object, TimeData, bool> fn)
        {
            _fn = fn;
        }
        
        public BehaviourTreeStatus Tick(object context, TimeData time)
        {
            return _fn(context, time) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
    }
}