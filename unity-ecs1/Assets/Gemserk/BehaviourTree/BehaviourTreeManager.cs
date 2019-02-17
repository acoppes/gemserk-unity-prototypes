using FluentBehaviourTree;

namespace Gemserk.BehaviourTree
{
    public interface BehaviourTreeManager
    {
        IBehaviourTreeNode GetTree(string name);
        void Add(string name, IBehaviourTreeNode node);
        BehaviourTreeContext GetContext();
        void SetContext(BehaviourTreeContext context);
    }
}