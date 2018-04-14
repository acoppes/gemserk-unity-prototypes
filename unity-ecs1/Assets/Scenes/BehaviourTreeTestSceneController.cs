using UnityEngine;
using FluentBehaviourTree;
using VirtualVillagers;

public class BehaviourTreeTestSceneController : MonoBehaviour {

    public UnityEngine.Object _behaviourTreeManager;

	public GameObject spawnPrefab;

	// Update is called once per frame
	void Awake() {
        var btManager = _behaviourTreeManager as BehaviourTreeManager;
		
		var spawnerTree = new BehaviourTreeBuilder()
			.Sequence("Spawner")
				.Do("WaitSomeTime", delegate(TimeData time)
				{
					var gameObject = btManager.GetContext() as GameObject;
					var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
					btContext.spawnIdleCurrentTime -= time.deltaTime;
					return btContext.spawnIdleCurrentTime > 0 ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
				})
				.Do("SpawnAndRestart", delegate(TimeData time)
				{
					var gameObject = btManager.GetContext() as GameObject;
					var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
					var spawnPosition = UnityEngine.Random.insideUnitCircle * 10.0f;
					var spawnItem = GameObject.Instantiate(spawnPrefab);
					spawnItem.transform.position = spawnPosition;
					btContext.spawnIdleCurrentTime = btContext.spawnIdleTotalTime;
					return BehaviourTreeStatus.Success;
				})
			.End()
			.Build();
		
		btManager.Add("Spawner", spawnerTree);

		var idleTree = new BehaviourTreeBuilder()
			.Sequence("IdleSequence")
			.Do("WaitSomeTime", delegate(TimeData time)
			{
				var gameObject = btManager.GetContext() as GameObject;
				var movement = gameObject.GetComponent<MovementComponent>();
				movement.currentIdleTime -= time.deltaTime;
				movement.direction.x = 0;
				movement.direction.y = 0;
				return movement.currentIdleTime > 0 ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
			})
			.Do("ResetLastDestination", delegate(TimeData time)
			{
				var gameObject = btManager.GetContext() as GameObject;
				var movement = gameObject.GetComponent<MovementComponent>();
				movement.hasDestination = false;
				movement.currentIdleTime = movement.idleTime;
				return BehaviourTreeStatus.Success;
			})
			.End()
			.Build();

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
						// movement.currentIdleTime = movement.idleTime;
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
				.Node(idleTree)
			.End()
			.Build());
		
		// Siguiente prueba: agregar una condicion de cooldown y escribir en el contexto
        // Tener una accion separada para incrementar el cooldown? 
        // (o bien chequear por un dato general en el contexto, tipo el "frame" de ejecución)
		
		// se puede construir action custom? No -> construir propias
		
		// lugar de datos comun para usar en los behaviour (actions)
		// lugar propio (contexto local, independiente del tree, pertenece a la unidad)
		
		// tener un buen debug de esto es escencial
    }
}
