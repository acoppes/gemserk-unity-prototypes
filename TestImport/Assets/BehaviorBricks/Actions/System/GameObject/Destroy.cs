using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/Destroy")]
    [Help("Destroys the gameobject")]
    public class Destroy : GOAction
    {
        [InParam("game object")]
        [Help("Game object to be destroyed, if no assigned the game object of this behavior will be destroyed")]
        public GameObject targetGameobject;

        public override void OnStart()
        {
            if (targetGameobject == null)
                targetGameobject = gameObject;
            GameObject.Destroy(targetGameobject);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
