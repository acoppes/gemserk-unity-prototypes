using UnityEngine;

namespace VirtualVillagers
{
    public class BehaviourTreeComponent : MonoBehaviour, IBehaviourTreeContext
    {
        public UnityEngine.Object _behaviourTreeManager;
        public string _behaviourTreeName;

        public T Get<T>(string name) where T : class
        {
            if (name.Equals("gameObject"))
                return gameObject as T;
            return null;
        }

        public void Process(float dt)
        {
            var btManager = _behaviourTreeManager as BehaviourTreeManager;
            if (btManager == null)
                return;
            var tree = btManager.GetTree(_behaviourTreeName);
            if (tree == null)
                return;
            btManager.SetContext(this);
            tree.Tick(new FluentBehaviourTree.TimeData(dt));
        }
    }
}
