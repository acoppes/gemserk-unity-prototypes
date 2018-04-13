using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms2D;
using FluentBehaviourTree;
using VirtualVillagers;

public class BehaviourTreeTestSceneController : MonoBehaviour {

    public UnityEngine.Object _behaviourTreeManager;

	// Update is called once per frame
	void Awake() {
        var btManager = _behaviourTreeManager as BehaviourTreeManager;

        btManager.Add("MoveRightTree", new BehaviourTreeBuilder()
            .Sequence("TestSequence")
                .Do("MyFirstAction", delegate (TimeData time) {
                    var context = btManager.GetContext();
                    var gameObject = context.Get<GameObject>("gameObject");
                    var movement = gameObject.GetComponent<MovementComponent>();
                    movement.direction.x = 1;
                    return BehaviourTreeStatus.Success;
                })
            .End()
            .Build());

        btManager.Add("MoveLeftTree", new BehaviourTreeBuilder()
            .Sequence("TestSequence")
                .Do("MyFirstAction", delegate (TimeData time) {
                    var context = btManager.GetContext();
                    var gameObject = context.Get<GameObject>("gameObject");
                    var movement = gameObject.GetComponent<MovementComponent>();
                    movement.direction.x = -1;
                    return BehaviourTreeStatus.Success;
                })
            .End()
            .Build());
    }
}
