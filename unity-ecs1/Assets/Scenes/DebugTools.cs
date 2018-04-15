using Unity.Mathematics;
using Unity.Transforms2D;
using UnityEngine;
using VirtualVillagers;

public class DebugTools : MonoBehaviour
{
	public GameObject treePrefab;
	public GameObject harvesterPrefab;
	
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
	}

	public void SpawnHarvester()
	{
		var spawnBounds = this.spawnBounds.bounds;
		
		var harvesterObject = GameObject.Instantiate(harvesterPrefab);
		harvesterObject.transform.position = new Vector2(
			spawnBounds.center.x + UnityEngine.Random.RandomRange(spawnBounds.min.x, spawnBounds.max.x), 
			spawnBounds.center.y + UnityEngine.Random.RandomRange(spawnBounds.min.y, spawnBounds.max.y)
		);

		var position2d = harvesterObject.GetComponent<Position2DComponent>();
		position2d.Value = new Position2D
		{
			Value = new float2(harvesterObject.transform.position.x, harvesterObject.transform.position.y)
		};
	}

}
