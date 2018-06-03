using System;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("GameObject/AddComponent")]
    [Help("Adds a component to the game object")]
    public class AddComponent : GOAction
    {
        [InParam("type")]
        [Help("Type of the component that must be added")]
        public string type;

        [InParam("game object")]
        [Help("Game object to add the component, if no assigned the component is added to the game object of this behavior")]
        public GameObject targetGameobject;

        public override void OnStart()
        {
            if (targetGameobject == null)
                targetGameobject = gameObject;
            if (targetGameobject.GetComponent(type) == null)
                targetGameobject.AddComponent(Type.GetType(type));
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
