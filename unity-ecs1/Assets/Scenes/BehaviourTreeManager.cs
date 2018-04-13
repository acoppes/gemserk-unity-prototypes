using FluentBehaviourTree;

public interface BehaviourTreeManager
{
    IBehaviourTreeNode GetTree(string name);
    void Add(string name, IBehaviourTreeNode node);
    IBehaviourTreeContext GetContext();
    void SetContext(IBehaviourTreeContext context);
}

public interface IBehaviourTreeContext
{
    T Get<T>(string name) where T : class;
}
