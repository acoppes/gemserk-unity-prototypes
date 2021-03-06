﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using VirtualVillagers;
using VirtualVillagers.Components;

public class DebugTools : MonoBehaviour
{
	public GameObject treePrefab;
	public GameObject harvesterPrefab;
	public GameObject foodPrefab;
	public GameObject lumbermillPrefab;
	public GameObject eaterPrefab;

	[SerializeField]
	protected GameObject _debugBTPrefab;
	
	public BoxCollider2D spawnBounds;

	public void SpawnTree()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var gameObject = GameObject.Instantiate(treePrefab);
		gameObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var size = UnityEngine.Random.RandomRange(0, 3);
			
		var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
		var tree = gameObject.GetComponent<VirtualVillagers.Components.TreeComponent>();

		if (treePrefab.GetComponent<VirtualVillagers.Components.TreeComponent>().seedPrefab == treePrefab)
		{
			tree.seedPrefab = treePrefab;
		}
		

		tree.currentSize = size;
		gameObject.GetComponent<LumberComponent>().current = (size + 1) * tree.lumberPerSize;
//		btContext.treeCurrentLumber = (size + 1) * btContext.treeLumberPerSize;
			
		var treeView = gameObject.GetComponent<VirtualVillagers.TreeView>();
		treeView.SetTreeData(gameObject);

		btContext.idleCurrentTime = UnityEngine.Random.RandomRange(0, btContext.idleTotalTime);
		
		var entity = gameObject.GetComponent<GameObjectEntity>();
		entity.EntityManager.AddComponentData(entity.Entity, new Position
		{
			Value = gameObject.transform.position
		});
		
		var btComponent = gameObject.GetComponent<BehaviourTreeComponent>();
		if (btComponent)
		{
			var debugBT = GameObject.Instantiate(_debugBTPrefab, gameObject.transform);
			debugBT.GetComponent<DebugBT>().behaviourTreeComponent = btComponent;
		}
	}

	public void SpawnHarvester()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var gameObject = GameObject.Instantiate(harvesterPrefab);
		gameObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var entity = gameObject.GetComponent<GameObjectEntity>();
		entity.EntityManager.AddComponentData(entity.Entity, new Position
		{
			Value = gameObject.transform.position
		});

		var btComponent = gameObject.GetComponent<BehaviourTreeComponent>();
		if (btComponent)
		{
            var debugBT = GameObject.Instantiate(_debugBTPrefab, gameObject.transform);
			debugBT.GetComponent<DebugBT>().behaviourTreeComponent = btComponent;
		}

        var btContext = gameObject.GetComponent<BehaviourTreeContextComponent>();
        btContext.SetData<HarvesterData>("harvesterData", new HarvesterData()
        {
            harvestLumberCurrentTree = null,
            harvestLumberMaxDistance = 10.0f,
            harvestLumberMinDistance = 1.5f,
        });
	}

	public void SpawnLumbermillNoReturn()
	{
		SpawnLumbermill();
	}

	public GameObject SpawnLumbermill()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var gameObject = GameObject.Instantiate(lumbermillPrefab);
		gameObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var entity = gameObject.GetComponent<GameObjectEntity>();
        var po = new Position()
        {
            Value = new float3(gameObject.transform.position)
        };

        entity.EntityManager.AddComponentData(entity.Entity, po);

		var btComponent = gameObject.GetComponent<BehaviourTreeComponent>();
		if (btComponent)
		{
			var debugBT = GameObject.Instantiate(_debugBTPrefab, gameObject.transform);
			debugBT.GetComponent<DebugBT>().behaviourTreeComponent = btComponent;
		}

		return gameObject;
	}
	
	public GameObject SpawnEater()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var gameObject = GameObject.Instantiate(eaterPrefab);
		gameObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var entity = gameObject.GetComponent<GameObjectEntity>();
		entity.EntityManager.AddComponentData(entity.Entity, new Position
		{
			Value = gameObject.transform.position
		});

		var btComponent = gameObject.GetComponent<BehaviourTreeComponent>();
		if (btComponent)
		{
			var debugBT = GameObject.Instantiate(_debugBTPrefab, gameObject.transform);
			debugBT.GetComponent<DebugBT>().behaviourTreeComponent = btComponent;
		}

		return gameObject;
	}
	public void SpawnFood()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var gameObject = GameObject.Instantiate(foodPrefab);
		gameObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var entity = gameObject.GetComponent<GameObjectEntity>();
		entity.EntityManager.AddComponentData(entity.Entity, new Position
		{
			Value = gameObject.transform.position
		});
		
		var btComponent = gameObject.GetComponent<BehaviourTreeComponent>();
		if (btComponent)
		{
			var debugBT = GameObject.Instantiate(_debugBTPrefab, gameObject.transform);
			debugBT.GetComponent<DebugBT>().behaviourTreeComponent = btComponent;
		}
	}

	public void KillEveryone()
	{
		var entities = GameObject.FindObjectsOfType<GameObjectEntity>();
		foreach (var entity in entities)
		{
			if (entity.GetComponent<LumberMillUIComponent>() != null)
				continue;
			GameObject.Destroy(entity.gameObject);
		}
	}
}
