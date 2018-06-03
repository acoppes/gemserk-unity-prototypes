using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/GetComponent")]
    [Help("Gets the component of a given type if the game object has one attached, null if it doesn't")]
    public class GetComponent : GOAction
    {
        [InParam("type")]
        [Help("Component type")]
        public string type;

        [OutParam("component")]
        [Help("Found component, null if the game object hasn't one attached")]
        public Component component;

        private float elapsedTime;

        public override void OnStart()
        {
            component = gameObject.GetComponent(type);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
