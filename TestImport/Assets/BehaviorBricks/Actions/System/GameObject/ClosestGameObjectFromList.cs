/*
namespace BBUnity.Actions
{

    [Action("GameObject/ClosestGameObjectFromList")]
    public class ClosestGameObjectFromList : GOAction
    {
        [InParam("list")]
        public List<GameObject> list;
        [OutParam("foundGameObject")]
        public GameObject foundGameObject;

        private float elapsedTime;

        public override void OnStart()
        {
            float dist = float.MaxValue;
            foreach(GameObject go in list)
            {
                float newdist = (go.transform.position + gameobject.transform.position).sqrMagnitude;
                if(newdist < dist)
                {
                    dist = newdist;
                    foundGameObject = go;
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}*/
