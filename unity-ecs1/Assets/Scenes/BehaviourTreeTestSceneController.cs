using System.Linq;
using UnityEngine;
using FluentBehaviourTree;
using VirtualVillagers;

public class BehaviourTreeTestSceneController : MonoBehaviour {

    public UnityEngine.Object _behaviourTreeManager;
	
	public int minTrees;
	public int maxTrees;

	public DebugTools debugTools;
	
	private void SpawnTrees()
	{
		var treesCount = UnityEngine.Random.Range(minTrees, maxTrees);
		
		for (var i = 0; i < treesCount; i++)
		{
			debugTools.SpawnTree();
		}
	}

	private void Start() {
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
		
		var harvestLumber = new BehaviourTreeBuilder()
			.Selector("All")
				.Sequence("Harvest")
					.Condition("NotAtMaximumLumber", delegate(TimeData time)
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						return btContext.harvestLumberCurrent < btContext.harvestLumberTotal;
					})
					.Condition("IsSelectedTreaAtHarvestDistance", delegate(TimeData time)
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();

						if (btContext.harvestLumberCurrentTree == null)
							return false;
						
						return Vector2.Distance(gameObject.transform.position, btContext.harvestLumberCurrentTree.transform.position) < 
						       btContext.harvestLumberMinDistance;
					})
					.Do("HarvestTree", time =>
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						
						var currentTree = btContext.harvestLumberCurrentTree;

						var currentTreeBtContext =currentTree.GetComponent<BehaviourTreeContextComponent>();

						var lumber = btContext.harvestLumberSpeed * time.deltaTime;

						lumber = Mathf.Min(lumber, currentTreeBtContext.treeCurrentLumber);
						
						currentTreeBtContext.treeCurrentLumber -= lumber;
						btContext.harvestLumberCurrent += lumber;

						if (currentTreeBtContext.treeCurrentLumber <= 0.0f)
						{
							GameObject.Destroy(currentTree);
							btContext.harvestLumberCurrentTree = null;
						}

						if (btContext.harvestLumberCurrent > btContext.harvestLumberTotal)
						{
							btContext.harvestLumberCurrent = btContext.harvestLumberTotal;
							btContext.harvestLumberCurrentTree = null;
						}

						return btContext.harvestLumberCurrentTree == null ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Running;
					})
				.End()
				.Sequence("MoveToTree")
					.Condition("NotAtMaximumLumber", delegate(TimeData time)
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						return btContext.harvestLumberCurrent < btContext.harvestLumberTotal;
					})
					.Do("SelectTree", delegate(TimeData time)
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
		
						if (btContext.harvestLumberCurrentTree != null)
						{
							return BehaviourTreeStatus.Success;
						}

						var trees = GameObject.FindGameObjectsWithTag("Tree");
						if (trees.Length == 0)
							return BehaviourTreeStatus.Failure;
						
						var nearByTrees = trees.Where(tree => Vector2.Distance(gameObject.transform.position, tree.transform.position) <
															  btContext.harvestLumberMaxDistance).ToList();
						if (nearByTrees.Count == 0)
							return BehaviourTreeStatus.Failure;
								
						btContext.harvestLumberCurrentTree = nearByTrees[UnityEngine.Random.Range(0, nearByTrees.Count)];
		
						return BehaviourTreeStatus.Success;
					})
					.Do("MoveToTree", delegate (TimeData time) {
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						var movement = gameObject.GetComponent<MovementComponent>();
						movement.SetDestination(btContext.harvestLumberCurrentTree.transform.position);
						return BehaviourTreeStatus.Success;
					}) 
					.SubTree(moveTo)
				.End()
					.Sequence("MoveToLumberMill")
						.Condition("MaximumLumber", delegate(TimeData time)
						{
							var gameObject = btManager.GetContext() as GameObject;
							var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
							return btContext.harvestLumberCurrent >= btContext.harvestLumberTotal;
						})
						// if at distance of lumber mill deposit lumber and reset
						.Do("FindLumberMill", delegate(TimeData time)
						{
							var gameObject = btManager.GetContext() as GameObject;
							var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
							var movement = gameObject.GetComponent<MovementComponent>();
					
							var lumberMills = GameObject.FindGameObjectsWithTag("LumberMill");
							if (lumberMills.Length == 0)
								return BehaviourTreeStatus.Failure;

							var lumberMill = lumberMills[0];

							if (Vector2.Distance(gameObject.transform.position, lumberMill.transform.position) <
							    movement.destinationDistance)
							{
								var lumberBtContext = lumberMill.GetComponent<BehaviourTreeContextComponent>();
								lumberBtContext.lumberMillLumberCurrent += btContext.harvestLumberCurrent;
								
								btContext.harvestLumberCurrent = 0;
								return BehaviourTreeStatus.Success;
							}
							
							movement.SetDestination(lumberMill.transform.position);
					
							return BehaviourTreeStatus.Success;
						})
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
		
		btManager.Add("LumberHarvester", new BehaviourTreeBuilder()
			.Selector("Selector")
				.SubTree(harvestLumber)
				.Sequence("Idle")
					.SubTree(notMovingCondition)
					.SubTree(idle)
				.End()
				.SubTree(wander)
			.End()
			.Build());
		
		btManager.Add("Tree", new BehaviourTreeBuilder()
			.Selector("Selector")
				// duda de como evitar que corra un comportamiento si estoy ejecutando otro
				// podría poner condiciones extra, tipo "not growing" && "not spawning seeds"
				// en algunas implementaciones vi lo del pending para animaciones/transiciones.
				.SubTree(idle)
				.Sequence("Grow")
					.Condition("NotHarvested", time =>
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						return btContext.treeCurrentLumber < btContext.treeLumberPerSize * (btContext.treeCurrentSize + 1);
					})
					.Condition("NotMaxSize", time =>
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						return btContext.treeCurrentSize < btContext.treeMaxSize;
					})
					.Do("Grow", time =>
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						btContext.treeCurrentSize++;

						// dudas de como y donde reflejar cambios visuales

						btContext.treeCurrentLumber = btContext.treeLumberPerSize * (btContext.treeCurrentSize + 1);
						return BehaviourTreeStatus.Success;
					})
				.End()
				.Sequence("SpawnSeeds")
					.Condition("NotHarvested", time =>
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						return btContext.treeCurrentLumber < btContext.treeLumberPerSize * (btContext.treeCurrentSize + 1);
					})
					.Condition("HasSeeds", time =>
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
						
						// what about checking global values like max Trees in game
						//  * static value probably? who is in charge of increasing/decreasing it?
						
						return btContext.treeSeeds > 0;
					})
					.Do("SpawnTree", time =>
					{
						var gameObject = btManager.GetContext() as GameObject;
						var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
	
						if (btContext.spawnPrefab == null)
							return BehaviourTreeStatus.Failure;
						
						var treeObject = GameObject.Instantiate(btContext.spawnPrefab);
	
						// TODO: consider there is already a tree in that location, use a grid for better locations also
						var randomPosition = UnityEngine.Random.insideUnitCircle * 
							UnityEngine.Random.RandomRange(btContext.treeMinSpawnDistance, btContext.treeMaxSpawnDistance);
						
						treeObject.transform.position = gameObject.transform.position 
														+ (Vector3) randomPosition;
					
						var treeBtContext = treeObject.GetComponent<BehaviourTreeContextComponent>();
						
						treeBtContext.treeCurrentSize = 0;
						treeBtContext.idleCurrentTime = treeBtContext.idleTotalTime;
						treeBtContext.treeCurrentLumber = treeBtContext.treeLumberPerSize * (treeBtContext.treeCurrentSize + 1);
	
						var tree = treeObject.GetComponent<VirtualVillagers.Tree>();
						tree.SetTreeData(treeBtContext);
						
						btContext.treeSeeds--;
						
						// dudas de como y donde reflejar cambios visuales
						return BehaviourTreeStatus.Success;
					})
				.End()
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

		// que pasa si quiero tener varios templates de arboles, o sea, el behaviour es el mismo pero 
		// quiero distintas configuraciones (variaciones de grow time, spawn child, etc)
		//		* podria tener distintos prefabs (wakale) y meter ahi un random de esos
		// 		* podría mover la config inicial a un scriptable object y usar un random de esos
		// 		* si fuera dato de entidad de ecs, podrían ser como templates/blueprints de esa entidad
		
		// los harvesters ahora se llenan y no hacen más nada, estaría bueno tener una lumber mill o algo
		// y un hud en pantalla, cuando estan llenos de leña que vayan ahi y la dejen.

		// Feedback al fluent
		// no se puede hacer un árbol de una leaf sola? (un subarbol)
		// no se pueden agregar subarboles con al builder (yo agregue un Node(), podría llamarse subtree)
	}
}
