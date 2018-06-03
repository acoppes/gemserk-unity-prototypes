using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/LookAt")]
    [Help("Rotates the transform so the forward vector of the game object points at target's current position")]
    public class LookAt : GOAction
    {
        [InParam("target")]
        [Help("Target game object")]
        public GameObject target;

        private Transform targetTransform;

        public override void OnStart()
        {
            if (target == null)
            {
                Debug.LogError("The look target of this game object is null", gameObject);
                return;
            }
            targetTransform = target.transform;
        }

        public override TaskStatus OnUpdate()
        {
            if (target == null)
                return TaskStatus.FAILED;
            Vector3 lookPos = targetTransform.position;
            gameObject.transform.LookAt(lookPos);
            return TaskStatus.COMPLETED;
        }
    }
}
