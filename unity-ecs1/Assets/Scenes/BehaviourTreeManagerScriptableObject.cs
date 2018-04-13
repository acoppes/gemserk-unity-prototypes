using FluentBehaviourTree;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Managers/BehaviourTreeManager")]
public class BehaviourTreeManagerScriptableObject : ScriptableObject, BehaviourTreeManager
{
    Dictionary<string, IBehaviourTreeNode> _trees = new Dictionary<string, IBehaviourTreeNode>();

    IBehaviourTreeContext _context;

    public void Add(string name, IBehaviourTreeNode t)
    {
        _trees[name] = t;
    }

    public IBehaviourTreeContext GetContext()
    {
        return _context;
    }

    public IBehaviourTreeNode GetTree(string name)
    {
        IBehaviourTreeNode node = null;
        _trees.TryGetValue(name, out node);
        return node;
    }

    public void SetContext(IBehaviourTreeContext context)
    {
        _context = context;
    }
}