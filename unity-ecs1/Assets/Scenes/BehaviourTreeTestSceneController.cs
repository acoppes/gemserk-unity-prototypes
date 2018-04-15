using UnityEngine;
using FluentBehaviourTree;
using VirtualVillagers;

public class BehaviourTreeTestSceneController : MonoBehaviour {

    public UnityEngine.Object _behaviourTreeManager;

//	public int SpawnMaxItemCount = 3;
//	public GameObject spawnPrefab;

	// Update is called once per frame
	void Awake() {
        var btManager = _behaviourTreeManager as BehaviourTreeManager;
		
		var moveTo = new BehaviourTreeBuilder()
			.Sequence("MoveSequence")
				.Do("Moveto", delegate(TimeData time)
				{
					var gameObject = btManager.GetContext() as GameObject;
					var movement = gameObject.GetComponent<MovementComponent>();
					
					var distance = Vector2.Distance(gameObject.transform.position, movement.destination);
					if (distance < movement.destinationDistance)
					{
						movement.hasDestination = false;
						return BehaviourTreeStatus.Success;
					}
					
					movement.direction.x = movement.destination.x - gameObject.transform.position.x;
					movement.direction.y = movement.destination.y - gameObject.transform.position.y;
					
					return BehaviourTreeStatus.Running;
				})
			.End()
			.Build();
		
		var spawner = new BehaviourTreeBuilder()
			.Sequence("Spawner")
				.Condition("NotAtMaximum", time =>
				{
					var gameObject = btManager.GetContext() as GameObject;
					var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
					if (btContext.spawnItemsMax < 0)
						return true;
					if (string.IsNullOrEmpty(btContext.spawnPrefab.tag))
						return true;
					var spawnedItems = GameObject.FindGameObjectsWithTag(btContext.spawnPrefab.tag);
					return spawnedItems.Length < btContext.spawnItemsMax;
				})
				.Do("WaitSomeTime", delegate(TimeData time)
				{
					var gameObject = btManager.GetContext() as GameObject;
					var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
					btContext.spawnIdleCurrentTime -= time.deltaTime;
					return btContext.spawnIdleCurrentTime > 0 ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
				})
				.Do("Spawn", delegate
				{
					var gameObject = btManager.GetContext() as GameObject;
					var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
					var spawnPosition = UnityEngine.Random.insideUnitCircle * 10.0f;
					var spawnItem = GameObject.Instantiate(btContext.spawnPrefab);
					spawnItem.transform.position = spawnPosition;
					btContext.spawnIdleCurrentTime = btContext.spawnIdleTotalTime;
					return BehaviourTreeStatus.Success;
				})
			.End()
			.Build();
		
		btManager.Add("Spawner", spawner);

		var idle = new BehaviourTreeBuilder()
			.Sequence("IdleSequence")
				.Do("WaitSomeTime", delegate(TimeData time)
				{
					var gameObject = btManager.GetContext() as GameObject;
					var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
					var movement = gameObject.GetComponent<MovementComponent>();
					
					if (movement.hasDestination)
						return BehaviourTreeStatus.Failure;
					
					btContext.idleCurrentTime -= time.deltaTime;
	
					if (btContext.idleCurrentTime > 0)
						return BehaviourTreeStatus.Running;
	
					btContext.idleCurrentTime = btContext.idleTotalTime;
					
					return BehaviourTreeStatus.Failure;
				})
			.End()
			.Build();
		
		var searchFood = new BehaviourTreeBuilder()
			.Sequence("SearchFood")
			.Condition("IsThereAnyFood", delegate(TimeData data)
			{
				var gameObject = btManager.GetContext() as GameObject;
				
				var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
				if (btContext.foodSelection != null)
					return true;
				
				var foodItems = GameObject.FindGameObjectsWithTag("Food");
				return foodItems.Length > 0;
			})
			.Do("SelectRandomFood", delegate(TimeData time)
			{
				var gameObject = btManager.GetContext() as GameObject;
				var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
				var movement = gameObject.GetComponent<MovementComponent>();

				if (btContext.foodSelection == null)
				{
					var foodItems = GameObject.FindGameObjectsWithTag("Food");
					btContext.foodSelection = foodItems[UnityEngine.Random.Range(0, foodItems.Length)];
				}

				movement.destination = btContext.foodSelection.transform.position;
				return BehaviourTreeStatus.Success;
			})
			.Node(moveTo)
			.Do("Consumefood", delegate(TimeData time)
			{
				var gameObject = btManager.GetContext() as GameObject;
				var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
				
				// consumes food
				GameObject.Destroy(btContext.foodSelection);
				btContext.foodSelection = null;
				btContext.foodConsumed++;
				
				return BehaviourTreeStatus.Success;
			})
			.End()
			.Build();

		var wander = new BehaviourTreeBuilder()
			.Selector("All")
				.Sequence("SetWanderDestination")
					.Condition("NotHasDestination", delegate(TimeData time)
					{
						var gameObject = btManager.GetContext() as GameObject;
						//				var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						var movement = gameObject.GetComponent<MovementComponent>();
						return !movement.hasDestination;
					})
					.Do("SetDestination", delegate (TimeData time) {
						var gameObject = btManager.GetContext() as GameObject;
						//				var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						var movement = gameObject.GetComponent<MovementComponent>();
						movement.SetDestination(UnityEngine.Random.insideUnitCircle * 10.0f);
						// movement.currentIdleTime = movement.idleTime;
						return BehaviourTreeStatus.Success;
					}) 
				.End()
				.Sequence("Wander")
					.Node(moveTo)
				.End()
			.End()
			.Build();

		btManager.Add("WandererAndEater", new BehaviourTreeBuilder()
			.Selector("Selector")
				.Node(searchFood)
				.Node(idle)
				.Node(wander)
			.End()
			.Build());


		btManager.Add("Wanderer", new BehaviourTreeBuilder()
			.Selector("Selector")
				.Node(idle)
				.Node(wander)
			.End()
			.Build());
		
		// Siguiente prueba: agregar una condicion de cooldown y escribir en el contexto
        // Tener una accion separada para incrementar el cooldown? 
        // (o bien chequear por un dato general en el contexto, tipo el "frame" de ejecución)
		
		// se puede construir action custom? No -> construir propias
		
		// lugar de datos comun para usar en los behaviour (actions)
		// lugar propio (contexto local, independiente del tree, pertenece a la unidad)
		
		// tener un buen debug de esto es escencial
		
		// Siguiente paso, ir a cortar leña y juntarla en unapila.
		// Los arboles crecen con el tiempo de min a max, y cada tamañno tiene un determinado cantidad de leña.
		// Solo crecen si no fueron harvesteados nunca.
		
		// harvestear lleva tiempo, es decir, el árbol tiene cierta resistencia para bajar de un nivel a otro
		
		// arboles crecen en tiempo
		// cuando son grandes, ponen otros arboles al rededor (si no hay arboles ya)
		
		// moveto parece comun entre distintos comportamientos (chop, food, wander)
		// como extraerlo?
    }
}
