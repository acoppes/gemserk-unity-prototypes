using FluentBehaviourTree;
using Gemserk.BehaviourTree;
using Unity.Entities;
using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers.Systems
{
    public class BehaviourTreeSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentArray<BehaviourTreeComponent> behaviourTree;
        }

        [Inject] private readonly Data m_Data;

        private BehaviourTreeManager _btManager;

        public void SetBehaviourTreeManager(BehaviourTreeManager btManager)
        {
            _btManager = btManager;
        }

        private UnityBehaviourTreeContext _context = new UnityBehaviourTreeContext();

        protected override void OnUpdate()
        {
            if (_btManager == null)
                return;
            
            var dt = Time.deltaTime;

            for (var i = 0; i < m_Data.Length; i++)
            {
                // m_Data.myComponent[i].superValue++;

                var bt = m_Data.behaviourTree[i];
                
                var tree = _btManager.GetTree(bt._behaviourTreeName);
                if (tree == null)
                    return;

                ActionNode.DebugCurrentNode = "None";

                _btManager.SetContext(_context);
                _context.SetGameObject(bt.gameObject);
                
                // _btManager.SetContext(bt.gameObject);
                tree.Tick(new FluentBehaviourTree.TimeData(dt));
                _btManager.SetContext(null);
                
                // just for debug!
                bt._debugCurrentAction = ActionNode.DebugCurrentNode;
            }
        }
    }
}