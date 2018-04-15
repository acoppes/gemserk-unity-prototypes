using UnityEngine;
using FluentBehaviourTree;
using VirtualVillagers;

public class BehaviourTreeTestSceneController : MonoBehaviour {

    public UnityEngine.Object _behaviourTreeManager;

//	public int SpawnMaxItemCount = 3;
//	public GameObject spawnPrefab;

	public GameObject treePrefab;
	
	public int minTrees;
	public int maxTrees;

	public BoxCollider2D treeSpawnerBounds;

	private void SpawnTrees()
	{
		var treesCount = UnityEngine.Random.Range(minTrees, maxTrees);
		var spawnBounds = treeSpawnerBounds.bounds;
		
		for (var i = 0; i < treesCount; i++)
		{
			var treeObject = GameObject.Instantiate(treePrefab);
			treeObject.transform.position = new Vector2(
				spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
				spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
			);
			var tree = treeObject.GetComponent<VirtualVillagers.Tree>();
			tree.SetSize(UnityEngine.Random.RandomRange(0, 3));
		}
		
		treeSpawnerBounds.gameObject.SetActive(false);
	}

	// Update is called once per frame
	private void Awake() {
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

		var notMovingCondition = new BehaviourTreeBuilder()
			.Sequence("NotMoving")
				.Condition("NotMoving", time => {
					var gameObject = btManager.GetContext() as GameObject;
					var movement = gameObject.GetComponent<MovementComponent>();

					if (movement == null) 
						return true;
				
					return !movement.hasDestination;
				})
			.End()
			.Build();
		
		var idle = new BehaviourTreeBuilder()
			.Sequence("IdleSequence")
				.Do("WaitSomeTime", delegate(TimeData time)
				{
					var gameObject = btManager.GetContext() as GameObject;
					var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
					
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
			.SubTree(moveTo)
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
					.SubTree(moveTo)
				.End()
			.End()
			.Build();

		btManager.Add("WandererAndEater", new BehaviourTreeBuilder()
			.Selector("Selector")
				.SubTree(searchFood)
				.Sequence("Idle")
					.SubTree(notMovingCondition)
					.SubTree(idle)
				.End()
				.SubTree(wander)
			.End()
			.Build());


		btManager.Add("Wanderer", new BehaviourTreeBuilder()
			.Selector("Selector")
				.Sequence("Idle")
					.SubTree(notMovingCondition)
					.SubTree(idle)
				.End()
				.SubTree(wander)
			.End()
			.Build());
		
		btManager.Add("Tree", new BehaviourTreeBuilder()
			.Selector("Selector")
				.SubTree(idle)
			// if tree not at maximum
//				.Node(treeGrow)
			// if tree at maximum	
				// if tree seeds > 0
				// wait some time
				// spawn trees nearby
				// reduce seeds
			.End()
			.Build());

		SpawnTrees();

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

		// me rechina un poco el configurar el manager de behaviour trees, debería estar en el system y setearlo o 
		// pasarlo de parámetro? De paso, debería ser el system el que hace toda la lógica que hace el bt component


		// Feedback al fluent
		// no se puede hacer un árbol de una leaf sola? (un subarbol)
		// no se pueden agregar subarboles con al builder (yo agregue un Node(), podría llamarse subtree)
	}
}
