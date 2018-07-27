using System.Collections;
using UnityEngine;

public class PerformanceSceneController : MonoBehaviour
{
	public GameObject unitPrefab;

	public int unitsCount;

	public Collider2D spawnBounds;

	public float minVision;
	public float maxVision;

	public Transform unitsParent;
	
	// Use this for initialization
	private void Start ()
	{
		Application.targetFrameRate = 60;
		SpawnUnits(unitsCount);
	}

	public void SpawnUnits(int count)
	{
		var waypoints = FindObjectsOfType<Waypoint>();
		var visionSystem = FindObjectOfType<VisionSystem>();
		
		for (var i = 0; i < count; i++)
		{
			var x = UnityEngine.Random.Range(-spawnBounds.bounds.extents.x, spawnBounds.bounds.extents.x);
			var y = UnityEngine.Random.Range(-spawnBounds.bounds.extents.y, spawnBounds.bounds.extents.y);
			var unitObject = GameObject.Instantiate(unitPrefab, unitsParent);
			unitObject.transform.position = new Vector3(x, y, 0);

			var waypointMovement = unitObject.GetComponentInChildren<WaypointMovement>();
			waypointMovement.SetWaypoint(waypoints[UnityEngine.Random.Range(0, waypoints.Length)]);

			var vision = unitObject.GetComponentInChildren<Vision>();
			vision.player = UnityEngine.Random.Range(0, visionSystem.totalPlayers);
			vision.range = UnityEngine.Random.Range(minVision, maxVision);

			if (vision.player == 0)
			{
				unitObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
			}
			else
			{
				unitObject.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
			}

			StartCoroutine(UpdateUnitGround(vision));
		}
	}

	private IEnumerator UpdateUnitGround(Vision vision)
	{
		while (true)
		{
			yield return new WaitForFixedUpdate();

			var position = vision.transform.position;
			
			var collider = Physics2D.OverlapPoint(position);
			if (collider == null)
			{
				vision.groundLevel = 0;
				continue;
			}
			
			var obstacle = collider.GetComponent<VisionObstacle>();
			if (obstacle == null) 
				continue;
			
			vision.groundLevel = obstacle.groundLevel + 1;
		}
	}
}
