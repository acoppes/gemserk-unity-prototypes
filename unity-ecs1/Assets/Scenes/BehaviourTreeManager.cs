using FluentBehaviourTree;

public interface BehaviourTreeManager
{
    IBehaviourTreeNode GetTree(string behaviourTreeName);
    void Add(string v, IBehaviourTreeNode tree1);
}
