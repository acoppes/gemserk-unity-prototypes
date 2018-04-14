using System;

namespace FluentBehaviourTree
{
    public class BaseConditionCallbackNode : IBehaviourTreeNode
    {
        private readonly Func<TimeData, bool> _fn;
        
        
        public BaseConditionCallbackNode(Func<TimeData, bool> fn)
        {
            _fn = fn;
        }
        
        public BehaviourTreeStatus Tick(TimeData time)
        {
            return _fn(time) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
    }
}