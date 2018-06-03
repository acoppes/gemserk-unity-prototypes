using FluentBehaviourTree;

namespace Gemserk.BehaviourTree
{
    public interface BehaviourTreeManager
    {
        IBehaviourTreeNode GetTree(string name);
        void Add(string name, IBehaviourTreeNode node);
        object GetContext();
        void SetContext(object context);
    }
}