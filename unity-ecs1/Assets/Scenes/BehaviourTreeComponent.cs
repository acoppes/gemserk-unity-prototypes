using UnityEngine;

namespace VirtualVillagers
{
    public class BehaviourTreeComponent : MonoBehaviour
    {
        public UnityEngine.Object _behaviourTreeManager;
        public string _behaviourTreeName;
      
        public void Process(float dt)
        {
            var btManager = _behaviourTreeManager as BehaviourTreeManager;
            if (btManager == null)
                return;
            var tree = btManager.GetTree(_behaviourTreeName);
            if (tree == null)
                return;
            tree.Tick(new FluentBehaviourTree.TimeData(dt));
        }
    }
}
