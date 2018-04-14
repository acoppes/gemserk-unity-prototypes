using UnityEngine;

namespace VirtualVillagers
{
    public class BehaviourTreeComponent : MonoBehaviour
    {
        public UnityEngine.Object _behaviourTreeManager;
        public string _behaviourTreeName;
        
        // entity context

        public void Process(float dt)
        {
            var btManager = _behaviourTreeManager as BehaviourTreeManager;
            if (btManager == null)
                return;
            var tree = btManager.GetTree(_behaviourTreeName);
            if (tree == null)
                return;
            btManager.SetContext(gameObject);
            tree.Tick(new FluentBehaviourTree.TimeData(dt));
            btManager.SetContext(null);
        }
    }
}
