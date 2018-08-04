﻿using System.Collections;
using UnityEngine;

public class PerformanceSceneController : MonoBehaviour
{
	public GameObject unitPrefab;

	public int unitsCount;

	public Collider2D spawnBounds;

	public float minVision;
	public float maxVision;

	public Transform unitsParent;

	private VisionSystem _visionSystem;
	
	// Use this for initialization
	private void Start ()
	{
		_visionSystem = FindObjectOfType<VisionSystem>();
		
		Application.targetFrameRate = 60;
		SpawnUnits(unitsCount);

		var debugPanelScript = FindObjectOfType<DebugPanelScript>();
		if (debugPanelScript != null)
		{
			debugPanelScript.AddButton("+10 units", delegate
			{
				SpawnUnits(10);
			});
			
			debugPanelScript.AddButton("-10 units", delegate
			{
				var toRemove = 10;
				while (unitsParent.childCount > 0 && toRemove > 0)
				{
					var unit = unitsParent.GetChild(UnityEngine.Random.Range(0, unitsParent.childCount));
					Destroy(unit.gameObject);
					toRemove--;
				}
			});
		}
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

			vision.StartCoroutine(UpdateUnitGround(vision));
		}
	}
	
	private readonly WaitForFixedUpdate _waitforFixedUpdate = new WaitForFixedUpdate();

	private IEnumerator UpdateUnitGround(Vision vision)
	{
		while (true)
		{
			yield return _waitforFixedUpdate;

			var position = vision.transform.position;

			var groundLevel = _visionSystem.GetGroundLevel(position);
			
			vision.groundLevel = groundLevel;
		}
	}
}
