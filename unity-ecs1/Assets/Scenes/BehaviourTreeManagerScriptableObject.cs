using FluentBehaviourTree;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Managers/BehaviourTreeManager")]
public class BehaviourTreeManagerScriptableObject : ScriptableObject, BehaviourTreeManager
{
    Dictionary<string, IBehaviourTreeNode> _trees = new Dictionary<string, IBehaviourTreeNode>();

    public void Add(string name, IBehaviourTreeNode t)
    {
        _trees[name] = t;
    }

    public IBehaviourTreeNode GetTree(string name)
    {
        IBehaviourTreeNode node = null;
        _trees.TryGetValue(name, out node);
        return node;
    }
}