﻿
namespace FluentBehaviourTree
{
    public abstract class BaseActionNode : IBehaviourTreeNode
    {
        private BehaviourTreeStatus _status = BehaviourTreeStatus.Failure;
        
        public BehaviourTreeStatus Tick(object context, TimeData time)
        {
            if (_status != BehaviourTreeStatus.Running)
                OnInitialize();
            _status = Update(context, time);
            if (_status != BehaviourTreeStatus.Running)
                OnTerminate();
            return _status;
        }

        protected virtual void OnInitialize()
        {
            
        }

        protected virtual void OnTerminate()
        {
            
        }
        
        protected abstract BehaviourTreeStatus Update(object context, TimeData time);
    }
}