using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
    [Condition("Perception/IsTargetCloseAndInSight")]
    [Help("Checks whether a target is close and in sight depending on a given distance and an angle")]
    public class IsTargetCloseAndInSight : GOCondition
    {
        [InParam("target")]
        [Help("Target to check the distance and angle")]
        public GameObject target;

        [InParam("angle")]
        [Help("The view angle to consider that the target is in sight")]
        public float angle;

        [InParam("closeDistance")]
        [Help("The maximun distance to consider that the target is close")]
        public float closeDistance;

		public override bool Check()
        {
            Vector3 dir = (target.transform.position - gameObject.transform.position);
            if (dir.sqrMagnitude > closeDistance * closeDistance)
                return false;
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.1f, 0), dir, out hit))
            {
                return hit.collider.gameObject == target && Vector3.Angle(dir, gameObject.transform.forward) < angle * 0.5f;
            }
            return false;
		}
    }
}