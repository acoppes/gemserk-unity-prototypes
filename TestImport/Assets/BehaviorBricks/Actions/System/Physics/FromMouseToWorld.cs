using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("Physics/FromMouseToWorld")]
    [Help("Gets the game object and position that is under the mouse cursor")]
    public class FromMouseToWorld : GOAction
    {
        [OutParam("selectedGameObject")]
        [Help("Game object under the mouse cursor")]
        public GameObject selectedGameObject;

        [OutParam("selectedPosition")]
        [Help("Position under the mouse cursor")]
        public Vector3 selectedPosition;

        [InParam("camera")]
        [Help("Camera that is currently used to rendering the scene. If not assigned Camera.main is used")]
        public Camera camera;

        [InParam("mask")]
        [Help("LayerMask with layers that must be considered relevant to calculate the game object and position under the mouse cursor")]
        public LayerMask mask;

        public override void OnStart()
        {
        }

        public override TaskStatus OnUpdate()
        {
            if (camera == null)
                camera = Camera.main;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, mask))
            {
                selectedPosition = hit.point;
                selectedGameObject = hit.collider.gameObject;

                return TaskStatus.COMPLETED;
            }
            return TaskStatus.FAILED;
        }
    }
}
