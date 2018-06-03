using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
    [Condition("Perception/IsTargetInSight")]
    [Help("Checks whether a target is in sight depending on a given angle")]
    public class IsTargetInSight : GOCondition
    {
        [InParam("target")]
        [Help("Target to check the angle")]
        public GameObject target;

        [InParam("angle")]
        [Help("The view angle to consider that the target is in sight")]
        public float angle;

		public override bool Check()
		{
            Vector3 dir = (target.transform.position - gameObject.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.1f, 0), dir, out hit))
            {
                return hit.collider.gameObject == target && Vector3.Angle(dir, gameObject.transform.forward) < angle * 0.5f;
            }
            return false;
		}
    }
}