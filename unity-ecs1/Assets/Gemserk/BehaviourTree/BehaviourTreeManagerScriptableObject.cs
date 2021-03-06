﻿using System.Collections.Generic;
using FluentBehaviourTree;
using UnityEngine;

namespace Gemserk.BehaviourTree
{
    [CreateAssetMenu(menuName="Managers/BehaviourTreeManager")]
    public class BehaviourTreeManagerScriptableObject : ScriptableObject, BehaviourTreeManager
    {
        private readonly Dictionary<string, IBehaviourTreeNode> _trees = new Dictionary<string, IBehaviourTreeNode>();

        private BehaviourTreeContext _context;

        public void Add(string name, IBehaviourTreeNode t)
        {
            _trees[name] = t;
        }

        public BehaviourTreeContext GetContext()
        {
            return _context;
        }

        public IBehaviourTreeNode GetTree(string name)
        {
            IBehaviourTreeNode node;
            _trees.TryGetValue(name, out node);
            return node;
        }

        public void SetContext(BehaviourTreeContext context)
        {
            _context = context;
        }
    }
}