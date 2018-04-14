namespace FluentBehaviourTree
{
    public abstract class BaseConditionNode : IBehaviourTreeNode
    {
        public BehaviourTreeStatus Tick(TimeData time)
        {
            return CheckCondition() ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }

        protected abstract bool CheckCondition();
    }
}