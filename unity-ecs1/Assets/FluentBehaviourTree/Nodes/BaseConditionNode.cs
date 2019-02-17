namespace FluentBehaviourTree
{
    public abstract class BaseConditionNode : IBehaviourTreeNode
    {
        public BehaviourTreeStatus Tick(object context, TimeData time)
        {
            return CheckCondition(context) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }

        protected abstract bool CheckCondition(object context);
    }
}