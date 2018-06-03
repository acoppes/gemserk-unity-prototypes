using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/SetActive")]
    [Help("Activates or deactivates the game object")]
    public class SetActive : GOAction
    {
        [InParam("active")]
        [Help("true if must be activate")]
        public bool active;

        [InParam("game object")]
        [Help("Game object to set the active value, if no assigned the active value will be set to the game object of this behavior")]
        public GameObject targetGameobject;

        public override void OnStart()
        {
            if (targetGameobject == null)
                targetGameobject = gameObject;
            targetGameobject.SetActive(active);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
