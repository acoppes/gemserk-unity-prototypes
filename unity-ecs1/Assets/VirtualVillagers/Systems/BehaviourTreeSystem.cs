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

                var context = _btManager.GetContext();
                context.SetObject(bt.gameObject);
                
                tree.Tick(new FluentBehaviourTree.TimeData(dt));
                
                // just for debug!
                bt._debugCurrentAction = ActionNode.DebugCurrentNode;
            }
        }
    }
}