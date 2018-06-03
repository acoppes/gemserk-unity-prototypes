using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("Physics/ApplyForce")]
    [Help("Adds a force to the game object. As a result the game object will start moving")]
    public class ApplyForce : GOAction
    {
        [InParam("toApplyForce")]
        [Help("Game object where the force is applied, if no assigned the force is applied to the game object of this behavior")]
        public GameObject toApplyForce;

        [InParam("force")]
        [Help("Force to be applied")]
        public Vector3 force;

        public override void OnStart()
        {
            if (toApplyForce == null)
                toApplyForce = gameObject;
            if (toApplyForce.GetComponent<Rigidbody>() == null)
                toApplyForce.AddComponent<Rigidbody>();
            toApplyForce.GetComponent<Rigidbody>().AddForce(force);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
