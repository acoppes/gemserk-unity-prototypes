using UnityEngine;
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
                    var gameObject = btManager.GetContext() as GameObject;
                    var movement = gameObject.GetComponent<MovementComponent>();
                    movement.direction.x = 1;
                    return BehaviourTreeStatus.Success;
                })
            .End()
            .Build());


		btManager.Add("MoveLeftTree", new BehaviourTreeBuilder()
			.Selector("TestSequence")
				.Sequence("SetWanderDestination")
					.Condition("NotHasDestination", delegate(TimeData time)
					{
						var gameObject = btManager.GetContext() as GameObject;
						var movement = gameObject.GetComponent<MovementComponent>();
						return !movement.hasDestination;
					})
					.Do("SetDestination", delegate (TimeData time) {
						var gameObject = btManager.GetContext() as GameObject;
						var movement = gameObject.GetComponent<MovementComponent>();
						movement.destination = UnityEngine.Random.insideUnitCircle * 10.0f;
						movement.hasDestination = true;
						return BehaviourTreeStatus.Success;
					}) 
				.End()
				.Sequence("Wander")
					.Condition("HasDestinationAndNotNear", delegate(TimeData time)
					{
						var gameObject = btManager.GetContext() as GameObject;
						var movement = gameObject.GetComponent<MovementComponent>();
						var distance = Vector2.Distance(gameObject.transform.position, movement.destination);
						return movement.hasDestination && distance > movement.destinationDistance;
					})
					.Do("MoveToDestination", delegate (TimeData time) {
						var gameObject = btManager.GetContext() as GameObject;
						var movement = gameObject.GetComponent<MovementComponent>();
						movement.direction.x = movement.destination.x - gameObject.transform.position.x;
						movement.direction.y = movement.destination.y - gameObject.transform.position.y;
						return BehaviourTreeStatus.Running;
					})
				.End()
				.Sequence("Idle")
					.Do("ClearDestination", delegate (TimeData time) {
						var gameObject = btManager.GetContext() as GameObject;
						var movement = gameObject.GetComponent<MovementComponent>();
						movement.hasDestination = false;
						return BehaviourTreeStatus.Success;
					})
				.End()
			.End()
			.Build());
		
		// Siguiente prueba: agregar una condicion de cooldown y escribir en el contexto
        // Tener una accion separada para incrementar el cooldown? 
        // (o bien chequear por un dato general en el contexto, tipo el "frame" de ejecución)
		
		// testear condicion de nuevo
		// se puede construir action custom?
    }
}
