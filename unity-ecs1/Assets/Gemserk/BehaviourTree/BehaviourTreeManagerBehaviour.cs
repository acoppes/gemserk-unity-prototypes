using System.Collections.Generic;
using FluentBehaviourTree;
using UnityEngine;

namespace Gemserk.BehaviourTree
{
    public class BehaviourTreeManagerBehaviour : MonoBehaviour, BehaviourTreeManager
    {
        private readonly Dictionary<string, IBehaviourTreeNode> _trees = new Dictionary<string, IBehaviourTreeNode>();

        private object _context;

        public void Add(string name, IBehaviourTreeNode t)
        {
            _trees[name] = t;
        }

        public object GetContext()
        {
            return _context;
        }

        public IBehaviourTreeNode GetTree(string name)
        {
            IBehaviourTreeNode node;
            _trees.TryGetValue(name, out node);
            return node;
        }

        public void SetContext(object context)
        {
            _context = context;
        }
    }
}