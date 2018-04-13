using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms2D;
using FluentBehaviourTree;

public class BehaviourTreeTestSceneController : MonoBehaviour {

    public UnityEngine.Object _behaviourTreeManager;

	// Update is called once per frame
	void Awake() {
        var btManager = _behaviourTreeManager as BehaviourTreeManager;
        var tree1 = new BehaviourTreeBuilder()
            .Sequence("TestSequence")
                .Do("MyFirstAction", delegate (TimeData time) {
                    Debug.Log("processing!!");
                    return BehaviourTreeStatus.Success;
                })
            .End()
            .Build();

        btManager.Add("tree1", tree1);
    }
}
