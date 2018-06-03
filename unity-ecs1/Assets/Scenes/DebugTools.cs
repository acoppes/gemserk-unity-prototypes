using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;
using VirtualVillagers;
using VirtualVillagers.Components;

public class DebugTools : MonoBehaviour
{
	public GameObject treePrefab;
	public GameObject harvesterPrefab;
	public GameObject foodPrefab;
	
	public BoxCollider2D spawnBounds;

	public void SpawnTree()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var treeObject = GameObject.Instantiate(treePrefab);
		treeObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var size = UnityEngine.Random.RandomRange(0, 3);
			
		var btContext = treeObject.GetComponent<BehaviourTreeContextComponent>();
		btContext.treeCurrentSize = size;
		btContext.treeCurrentLumber = (size + 1) * btContext.treeLumberPerSize;
			
		var tree = treeObject.GetComponent<VirtualVillagers.Tree>();
		tree.SetTreeData(btContext);

		btContext.idleCurrentTime = UnityEngine.Random.RandomRange(0, btContext.idleTotalTime);
		
		var entity = treeObject.GetComponent<GameObjectEntity>();
		entity.EntityManager.SetComponentData(entity.Entity, new Position
		{
			Value = treeObject.transform.position
		});
	}

	public void SpawnHarvester()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var harvesterObject = GameObject.Instantiate(harvesterPrefab);
		harvesterObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var entity = harvesterObject.GetComponent<GameObjectEntity>();
		entity.EntityManager.SetComponentData(entity.Entity, new Position
		{
			Value = harvesterObject.transform.position
		});
	}

	public void SpawnFood()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var foodObject = GameObject.Instantiate(foodPrefab);
		foodObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var entity = foodObject.GetComponent<GameObjectEntity>();
		entity.EntityManager.SetComponentData(entity.Entity, new Position
		{
			Value = foodObject.transform.position
		});
	}

	public void KillEveryone()
	{
		var entities = GameObject.FindObjectsOfType<GameObjectEntity>();
		foreach (var entity in entities)
		{
			GameObject.Destroy(entity.gameObject);
		}
	}
}
