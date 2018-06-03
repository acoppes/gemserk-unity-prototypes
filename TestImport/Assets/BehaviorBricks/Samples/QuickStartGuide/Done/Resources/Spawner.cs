using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public GameObject prefab;
    public Vector3 position;

    public GameObject wanderArea;
    public GameObject player;

    //public Text text;

	void Start() {
        GameObject instance = Instantiate(prefab,position,Quaternion.identity) as GameObject;
        BehaviorExecutor behaviorExecutor = instance.GetComponent<BehaviorExecutor>();


		//Codigo comentado para comprobaciones de editor y runtime

        //if (BBUnity.Managers.BBManager.Instance.IsEditor)
        //    text.text = "EDITOR";
        //else
        //    text.text = "RUNTIME";

        if (behaviorExecutor != null)
        {
            behaviorExecutor.SetBehaviorParam("wanderArea", wanderArea);
            behaviorExecutor.SetBehaviorParam("player", player);
        }
	}
}
