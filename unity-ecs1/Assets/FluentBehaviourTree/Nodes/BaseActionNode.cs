
namespace FluentBehaviourTree
{
    public abstract class BaseActionNode : IBehaviourTreeNode
    {
        private BehaviourTreeStatus _status = BehaviourTreeStatus.Failure;
        
        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (_status != BehaviourTreeStatus.Running)
                OnInitialize();
            _status = Update(time);
            if (_status != BehaviourTreeStatus.Running)
                OnTerminate();
        }

        protected virtual void OnInitialize()
        {
            
        }

        protected virtual void OnTerminate()
        {
            
        }
        
        protected abstract BehaviourTreeStatus Update(TimeData time);
    }
}