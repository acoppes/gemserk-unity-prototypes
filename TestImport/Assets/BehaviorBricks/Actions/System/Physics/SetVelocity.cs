using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("Physics/SetVelocity")]
    [Help("Sets a velocity to the game object. As a result the game object will start moving")]
    public class SetVelocity : GOAction
    {
        [InParam("toSetVelocity")]
        [Help("Game object where the velocity is set, if no assigned the velocity is set to the game object of this behavior")]
        public GameObject toSetVelocity;

        [InParam("velocity")]
        [Help("Velocity")]
        public Vector3 velocity;

        public override void OnStart()
        {
            if (toSetVelocity == null)
                toSetVelocity = gameObject;
            if (toSetVelocity.GetComponent<Rigidbody>() == null)
                toSetVelocity.AddComponent<Rigidbody>();
            toSetVelocity.GetComponent<Rigidbody>().velocity = velocity;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
