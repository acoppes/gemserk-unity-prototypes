using System.Linq;
using UnityEngine;
using FluentBehaviourTree;
using Gemserk.BehaviourTree;
using Unity.Entities;
using VirtualVillagers.Components;

public static class BehaviourTreeContextExtensions
{
    public static SpawnerData GetSpawnerData(this BehaviourTreeContextComponent c)
    {
        return c.GetData<SpawnerData>("spawnerData");
    }

    public static HarvesterData GetHarvesterData(this BehaviourTreeContextComponent c)
    {
        return c.GetData<HarvesterData>("harvesterData");
    }
}

public static class BehavioursFactory
{
    public static void CreateDefaultBehaviours(BehaviourTreeManager bt)
    {
        var moveTo = new BehaviourTreeBuilder()
            .Sequence("MoveSequence")
            .Do("Moveto", delegate (object c, TimeData time)
            {
                var context = c as UnityBehaviourTreeContext;
                var movement = context.GetComponent<MovementComponent>();
                var transform = context.GetComponent<Transform>();

                // Maybe having something like GetPosition(Entity/Target) or stuff like that..
                // SetTarget(Query result)...

                // Distance(GetPosition(GetTarget()), GetPosition(GetOwner()) < Float(3.0f)

                var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                var distance = Vector2.Distance(transform.position, movement.destination);
                if (distance < movement.destinationDistance)
                {
                    btContext.actionState = BehaviourTreeContextComponent.ActionState.Idle;
                    movement.hasDestination = false;
                    return BehaviourTreeStatus.Success;
                }

                movement.direction.x = movement.destination.x - transform.position.x;
                movement.direction.y = movement.destination.y - transform.position.y;

                btContext.actionState = BehaviourTreeContextComponent.ActionState.Moving;

                return BehaviourTreeStatus.Running;
            })
            .End()
        .Build();

        var spawner = new BehaviourTreeBuilder()
            .Sequence("Spawner")
                .Condition("NotAtMaximum", (c, time) =>
                {
                    var context = c as UnityBehaviourTreeContext;
                    var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                    var spawnerData = btContext.GetSpawnerData();

                    if (spawnerData.spawnItemsMax < 0)
                        return true;
                    if (string.IsNullOrEmpty(spawnerData.spawnPrefab.tag))
                        return true;
                    var spawnedItems = GameObject.FindGameObjectsWithTag(spawnerData.spawnPrefab.tag);
                    return spawnedItems.Length < spawnerData.spawnItemsMax;
                })
                .Do("WaitSomeTime", delegate (object c, TimeData time)
                {
                    var context = c as UnityBehaviourTreeContext;
                    var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                    var spawnerData = btContext.GetSpawnerData();

                    spawnerData.spawnIdleCurrentTime -= time.deltaTime;
                    return spawnerData.spawnIdleCurrentTime > 0 ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
                })
                .Do("Spawn", (c, t) =>
                {
                    var context = c as UnityBehaviourTreeContext;
                    var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                    var spawnerData = btContext.GetSpawnerData();

                    var spawnPosition = UnityEngine.Random.insideUnitCircle * 10.0f;
                    var spawnItem = GameObject.Instantiate(spawnerData.spawnPrefab);
                    spawnItem.transform.position = spawnPosition;
                    spawnerData.spawnIdleCurrentTime = spawnerData.spawnIdleTotalTime;
                    return BehaviourTreeStatus.Success;
                })
            .End()
            .Build();

        bt.Add("Spawner", spawner);

        var notMovingCondition = new BehaviourTreeBuilder()
            .Sequence("NotMoving")
                .Condition("NotMoving", (c, time) => {

                    var context = c as UnityBehaviourTreeContext;
                    var movement = context.GetComponent<MovementComponent>();

                    if (movement == null)
                        return true;

                    return !movement.hasDestination;
                })
            .End()
            .Build();

        var idle = new BehaviourTreeBuilder()
            .Sequence("IdleSequence")
                .Do("WaitSomeTime", delegate (object c, TimeData time)
                {
                    var context = c as UnityBehaviourTreeContext;
                    var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                    btContext.idleCurrentTime -= time.deltaTime;

                    if (btContext.idleCurrentTime > 0) {
                        btContext.actionState = BehaviourTreeContextComponent.ActionState.Idle;
                        return BehaviourTreeStatus.Running;
                    }

                    btContext.idleCurrentTime = btContext.idleTotalTime;

                    return BehaviourTreeStatus.Failure;
                })
            .End()
            .Build();

        var searchFood = new BehaviourTreeBuilder()
            .Sequence("SearchFood")
            .Condition("IsThereAnyFood", delegate (object c, TimeData data)
            {
                var context = c as UnityBehaviourTreeContext;
                var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                if (btContext.foodSelection != null)
                    return true;

                var foodItems = GameObject.FindGameObjectsWithTag("Food");
                return foodItems.Length > 0;
            })
            .Do("SelectRandomFood", delegate (object c, TimeData time)
            {
                var context = c as UnityBehaviourTreeContext;
                var btContext = context.GetComponent<BehaviourTreeContextComponent>();
                var movement = context.GetComponent<MovementComponent>();

                if (btContext.foodSelection == null)
                {
                    var foodItems = GameObject.FindGameObjectsWithTag("Food");
                    btContext.foodSelection = foodItems[UnityEngine.Random.Range(0, foodItems.Length)];
                }

                movement.destination = btContext.foodSelection.transform.position;
                return BehaviourTreeStatus.Success;
            })
            .Splice(moveTo)
            .Do("Consumefood", delegate (object c, TimeData time)
            {
                var context = c as UnityBehaviourTreeContext;
                var btContext = context.GetComponent<BehaviourTreeContextComponent>();

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
                    .Condition("NotHasDestination", delegate (object c, TimeData time)
                    {
                        var context = c as UnityBehaviourTreeContext;
                        var btContext = context.GetComponent<BehaviourTreeContextComponent>();
                        var movement = context.GetComponent<MovementComponent>();

                        return !movement.hasDestination;
                    })
                    .Do("SetDestination", delegate (object c, TimeData time) {
                        var context = c as UnityBehaviourTreeContext;
                        var btContext = context.GetComponent<BehaviourTreeContextComponent>();
                        var movement = context.GetComponent<MovementComponent>();
                        movement.SetDestination(UnityEngine.Random.insideUnitCircle * 10.0f);
                        // movement.currentIdleTime = movement.idleTime;
                        return BehaviourTreeStatus.Success;
                    })
                .End()
                .Sequence("Wander")
                    .Splice(moveTo)
                .End()
            .End()
            .Build();

        var harvestLumber = new BehaviourTreeBuilder()
            .Selector("All")
                .Sequence("DeployLumber")
                .Condition("HasLumber", delegate (object c, TimeData time)
                {
                    // has at least 1 of lumber
                    var context = c as UnityBehaviourTreeContext;
                    var harvester = context.GetComponent<HarvesterComponent>();
                    return harvester.currentLumber > 0;
                })
                .Do("DeployLumber", (c, time) =>
                {
                    // deploy lumber 
                    var context = c as UnityBehaviourTreeContext;
                    var btContext = context.GetComponent<BehaviourTreeContextComponent>();
                    var movement = context.GetComponent<MovementComponent>();
                    var transform = context.GetComponent<Transform>();

                    // var lumberMills = GameObject.FindGameObjectsWithTag("LumberMill");

                    var gameWorld = context.GetManager<GameWorld>();
                    var lumberMills = gameWorld.GetLumberMills();

                    var validLumberMills = lumberMills.Where(lm =>
                    {
                        var lumberHolder = lm.GetComponent<LumberComponent>();
                        return lumberHolder.current < lumberHolder.total;
                    }).ToList();

                    if (validLumberMills.Count == 0)
                        return BehaviourTreeStatus.Failure;

                    validLumberMills.Sort((x1, x2) =>
                    {
                        var d1 = Mathf.RoundToInt(Vector2.Distance(transform.position, x1.transform.position));
                        var d2 = Mathf.RoundToInt(Vector2.Distance(transform.position, x2.transform.position));
                        return d1 - d2;
                    });

                    var lumberMill = validLumberMills[0];

                    // if not near enough to nearest lumbermill
                    if (Vector2.Distance(transform.position, lumberMill.transform.position) >
                        movement.destinationDistance)
                        return BehaviourTreeStatus.Failure;

                    var harvester = context.GetComponent<HarvesterComponent>();
                    harvester.currentLumberMill = lumberMill.GetComponent<GameObjectEntity>().Entity;

                    btContext.actionState = BehaviourTreeContextComponent.ActionState.Harvesting;

                    return BehaviourTreeStatus.Running;

                })
                .End()
                .Sequence("Harvest")
                    .Condition("NotAtMaximumLumber", delegate (object c, TimeData time)
                    {
                        var context = c as UnityBehaviourTreeContext;
                        var harvester = context.GetComponent<HarvesterComponent>();
                        return harvester.currentLumber < harvester.maxLumber;
                    })
                    .Condition("IsSelectedTreaAtHarvestDistance", delegate (object c, TimeData time)
                    {
                        var context = c as UnityBehaviourTreeContext;
                        var btContext = context.GetComponent<BehaviourTreeContextComponent>();
                        var movement = context.GetComponent<MovementComponent>();

                        var harvesterData = btContext.GetHarvesterData();
                        
                        if (harvesterData.harvestLumberCurrentTree == null)
                            return false;

                        var transform = context.GetComponent<Transform>();

                        return Vector2.Distance(transform.position, harvesterData.harvestLumberCurrentTree.transform.position) <
                               harvesterData.harvestLumberMinDistance;
                    })
                    .Do("HarvestTree", (c, time) =>
                    {
                        var context = c as UnityBehaviourTreeContext;
                        var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                        var harvesterData = btContext.GetHarvesterData();

                        var currentTree = harvesterData.harvestLumberCurrentTree;

                        var harvester = context.GetComponent<HarvesterComponent>();
                        if (harvester != null && harvesterData.harvestLumberCurrentTree != null)
                        {
                            harvester.currentLumberTarget =
                                harvesterData.harvestLumberCurrentTree.GetComponent<GameObjectEntity>().Entity;
                        }

                        if (harvesterData.harvestLumberCurrentTree != null)
                        {
                            btContext.actionState = BehaviourTreeContextComponent.ActionState.Harvesting;
                        }

                        return harvesterData.harvestLumberCurrentTree == null ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Running;
                    })
                .End()
                .Sequence("MoveToTree")
                    .Condition("NotAtMaximumLumber", delegate (object c, TimeData time)
                    {
                        var context = c as UnityBehaviourTreeContext;
                        var harvester = context.GetComponent<HarvesterComponent>();

                        return harvester.currentLumber < harvester.maxLumber;
                        //						return btContext.harvestLumberCurrent < btContext.harvestLumberTotal;
                    })
                    .Do("SelectTree", delegate (object c, TimeData time)
                    {
                        var context = c as UnityBehaviourTreeContext;
                        var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                        var harvesterData = btContext.GetHarvesterData();

                        if (harvesterData.harvestLumberCurrentTree != null)
                        {
                            return BehaviourTreeStatus.Success;
                        }

                        var trees = GameObject.FindGameObjectsWithTag("Tree");
                        if (trees.Length == 0)
                            return BehaviourTreeStatus.Failure;

                        var transform = context.GetComponent<Transform>();

                        var nearByTrees = trees.Where(tree => Vector2.Distance(transform.position, tree.transform.position) <
                                                              harvesterData.harvestLumberMaxDistance &&
                                                              tree.GetComponent<LumberComponent>().current > 0).ToList();
                        if (nearByTrees.Count == 0)
                            return BehaviourTreeStatus.Failure;

                        harvesterData.harvestLumberCurrentTree = nearByTrees[UnityEngine.Random.Range(0, nearByTrees.Count)];

                        return BehaviourTreeStatus.Success;
                    })
                    .Do("MoveToTree", delegate (object c, TimeData time) {
                        var context = c as UnityBehaviourTreeContext;
                        var btContext = context.GetComponent<BehaviourTreeContextComponent>();
                        var movement = context.GetComponent<MovementComponent>();
                        var harvesterData = btContext.GetHarvesterData();

                        movement.SetDestination(harvesterData.harvestLumberCurrentTree.transform.position);
                        return BehaviourTreeStatus.Success;
                    })
                    .Splice(moveTo)
                .End()
                    .Sequence("MoveToLumberMill")
                        .Condition("MaximumLumber", delegate (object c, TimeData time)
                        {
                            var context = c as UnityBehaviourTreeContext;
                            var harvester = context.GetComponent<HarvesterComponent>();
                            return harvester.currentLumber >= harvester.maxLumber;
                        })
                        // if at distance of lumber mill deposit lumber and reset
                        .Do("Move", delegate (object c, TimeData time)
                        {
                            var context = c as UnityBehaviourTreeContext;
                            var btContext = context.GetComponent<BehaviourTreeContextComponent>();
                            var movement = context.GetComponent<MovementComponent>();

                            // unity stuff inside the action code... 
                            // what if I want to use something like a QuadTree.. or World helper here?

                            var lumberMills = GameObject.FindGameObjectsWithTag("LumberMill");

                            var validLumberMills = lumberMills.Where(lm =>
                            {
                                var lumberHolder = lm.GetComponent<LumberComponent>();
                                return lumberHolder.current < lumberHolder.total;
                            }).ToList();

                            if (validLumberMills.Count == 0)
                                return BehaviourTreeStatus.Failure;

                            var transform = context.GetComponent<Transform>();

                            validLumberMills.Sort((x1, x2) =>
                            {
                                var d1 = Mathf.RoundToInt(Vector2.Distance(transform.position, x1.transform.position));
                                var d2 = Mathf.RoundToInt(Vector2.Distance(transform.position, x2.transform.position));
                                return d1 - d2;
                            });

                            var lumberMill = validLumberMills[0];

                            movement.SetDestination(lumberMill.transform.position);

                            return BehaviourTreeStatus.Success;
                        })
                        .Splice(moveTo)
                    .End()
                .End()
            .Build();

        bt.Add("WandererAndEater", new BehaviourTreeBuilder()
            .Selector("Selector")
                .Splice(searchFood)
                .Sequence("Idle")
                    .Splice(notMovingCondition)
                    .Splice(idle)
                .End()
                .Splice(wander)
            .End()
            .Build());


        bt.Add("Wanderer", new BehaviourTreeBuilder()
            .Selector("Selector")
                .Sequence("Idle")
                    .Splice(notMovingCondition)
                    .Splice(idle)
                .End()
                .Splice(wander)
            .End()
            .Build());

        bt.Add("LumberHarvester", new BehaviourTreeBuilder()
            .Selector("Selector")
                .Splice(harvestLumber)
                .Sequence("Idle")
                    .Splice(notMovingCondition)
                    .Splice(idle)
                .End()
                .Splice(wander)
            .End()
            .Build());

        bt.Add("Tree", new BehaviourTreeBuilder()
            .Selector("Selector")
                .Do("DoNothing", (c, time) => {
                    var context = c as UnityBehaviourTreeContext;
                    var btContext = context.GetComponent<BehaviourTreeContextComponent>();

                    var lumber = context.GetComponent<LumberComponent>();
                    var result = lumber.current <= 0 ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Failure;
                    //					var result = btContext.treeCurrentLumber <= 0 ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Failure;

                    if (result == BehaviourTreeStatus.Running)
                    {
                        var destroyableComponent = context.GetComponent<DestroyableComponent>();
                        if (destroyableComponent != null)
                            destroyableComponent.shouldDestroy = true;
                        return BehaviourTreeStatus.Success;
                    }

                    return result;
                })
                // duda de como evitar que corra un comportamiento si estoy ejecutando otro
                // podría poner condiciones extra, tipo "not growing" && "not spawning seeds"
                // en algunas implementaciones vi lo del pending para animaciones/transiciones.
                .Splice(idle)

            .End()
            .Build());
    }
}
