using System.Collections;
using Gemserk;
using Gemserk.Vision;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PerformanceSceneController : MonoBehaviour
{
	public GameObject unitPrefab;

	public int unitsCount;

	public Collider2D spawnBounds;

	public float minVision;
	public float maxVision;

	public Transform unitsParent;

	private VisionSystem _visionSystem;

	private int _currentPlayerIndex;

	private int[] _playerConfigs = new int[]
	{
		0, 1, 2, 4, 8, 3, 7, 15
	};
	
	[SerializeField]
	protected PostProcessingBehaviour _postProcessing;

	[SerializeField]
	protected VisionTerrainTexture _visionTerrain;
	
	[SerializeField]
	protected string _fogLayerName = "Fog";
	
	[SerializeField]
	protected string _defaultLayerName = "Default";
	
	public Color[] _playerColors = new Color[] 
	{
		Color.red,
		Color.blue,
		Color.yellow,
		Color.green
	};

	
	// Use this for initialization
	private void Start ()
	{
		_visionSystem = FindObjectOfType<VisionSystem>();
		
		Application.targetFrameRate = 60;
		SpawnUnits(unitsCount);

		var debugPanelScript = FindObjectOfType<DebugPanelScript>();
		if (debugPanelScript != null)
		{
			_currentPlayerIndex = 1;
			_visionSystem._activePlayers = _playerConfigs[_currentPlayerIndex];
			
			debugPanelScript.AddButton("switch players", delegate
			{
				_currentPlayerIndex = (_currentPlayerIndex + 1) % _playerConfigs.Length;
				_visionSystem._activePlayers = _playerConfigs[_currentPlayerIndex];				
			}, null);
			
			debugPanelScript.AddButton("add 10 units", delegate
			{
				SpawnUnits(10);
			}, null);
			
			debugPanelScript.AddButton("remove 10 units", delegate
			{
				var toRemove = 10;
				while (unitsParent.childCount > 0 && toRemove > 0)
				{
					var unit = unitsParent.GetChild(0);
					DestroyImmediate(unit.gameObject);
					toRemove--;
				}
			}, null);
			
			var visionTexture = FindObjectOfType<VisionTexture>();
			
			if (visionTexture != null)
			{
				var b = debugPanelScript.AddButton("fog interpolation", button =>
				{
					visionTexture.ColorInterpolation = !visionTexture.ColorInterpolation;
					button.UpdateText(string.Format("easing: {0}", visionTexture.ColorInterpolation ? "on" : "off"));
				}, null);
				b.UpdateText(string.Format("easing: {0}", visionTexture.ColorInterpolation ? "on" : "off"));
			}
			
			if (visionTexture != null)
			{
				var b = debugPanelScript.AddButton("previous vision", new DebugPanelButton.Actions()
				{
					callbackAction = button =>
					{
						visionTexture.PreviousVision = !visionTexture.PreviousVision;
					},
					refreshAction = button =>
					{
						button.UpdateText(string.Format("visited: {0}", visionTexture.PreviousVision ? "on" : "off"));
					}
				});
			}
			
			if (_visionSystem != null)
			{
				var b = debugPanelScript.AddButton("update method", button =>
				{
					_visionSystem.updateMethod = !_visionSystem.updateMethod;
					button.UpdateText(string.Format("method: {0}", _visionSystem.updateMethod ? "1" : "2"));
				}, null);
				
				b.UpdateText(string.Format("method: {0}", _visionSystem.updateMethod ? "1" : "2"));
			}
			
			if (_visionSystem != null)
			{
				var b = debugPanelScript.AddButton("raycast", button =>
				{
					_visionSystem.raycastEnabled = !_visionSystem.raycastEnabled;
					button.UpdateText(string.Format("raycast: {0}", _visionSystem.raycastEnabled ? "on" : "off"));
				}, null);
				
				b.UpdateText(string.Format("raycast: {0}", _visionSystem.raycastEnabled ? "on" : "off"));
			}
			
			if (_postProcessing != null)
			{
				var b = debugPanelScript.AddButton("blur", button =>
				{
					_postProcessing.enabled = !_postProcessing.enabled;
					button.UpdateText(string.Format("blur: {0}", _postProcessing.enabled ? "on" : "off"));
				}, null);
				
				b.UpdateText(string.Format("blur: {0}", _postProcessing.enabled ? "on" : "off"));
			}

			if (_visionSystem != null)
			{
				var b = debugPanelScript.AddButton("clear", button =>
				{
					_visionSystem.Clear();
				}, null);
			}

			if (_visionTerrain != null)
			{
				var fogLayerMask = LayerMask.NameToLayer(_fogLayerName);
				var defaultLayerMask = LayerMask.NameToLayer(_defaultLayerName);
				
				var b = debugPanelScript.AddButton("terrain", button =>
				{
					if (_visionTerrain.gameObject.layer == fogLayerMask)
					{
						_visionTerrain.gameObject.SetLayerRecursive(defaultLayerMask);	
					} else if (_visionTerrain.gameObject.layer == defaultLayerMask)
					{
						_visionTerrain.gameObject.SetLayerRecursive(fogLayerMask);
					}
					
					button.UpdateText(string.Format("terrain: {0}", _visionTerrain.gameObject.layer == defaultLayerMask ? "on" : "off"));
				}, null);
				
				b.UpdateText(string.Format("terrain: {0}", _visionTerrain.gameObject.layer == defaultLayerMask ? "on" : "off"));
			}
			
			debugPanelScript.AddLabel("unitCount", delegate(DebugPanelLabel label)
			{
				label.UpdateText(string.Format("units: {0}", unitsParent.childCount));
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

			var randomPlayer = UnityEngine.Random.Range(0, visionSystem.totalPlayers);
			
			var vision = unitObject.GetComponentInChildren<Vision>();
			vision.player = 1 << randomPlayer;
			vision.range = UnityEngine.Random.Range(minVision, maxVision);

			unitObject.GetComponentInChildren<SpriteRenderer>().color = _playerColors[randomPlayer];

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
