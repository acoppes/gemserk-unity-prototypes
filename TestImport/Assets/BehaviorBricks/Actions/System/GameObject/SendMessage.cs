using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/SendMessage")]
    [Help("Calls the method named methodName on every MonoBehaviour in this game object")]
    public class SendMessage : GOAction
    {
        [InParam("methodName")]
        [Help("Name of the method that must be called")]
        public string methodName;

        [InParam("game object")]
        [Help("Game object to send the message, if no assigned the message will be sent to the game object of this behavior")]
        public GameObject targetGameobject;

        public override void OnStart()
        {
            if (targetGameobject == null)
                targetGameobject = gameObject;
            targetGameobject.SendMessage(methodName);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
