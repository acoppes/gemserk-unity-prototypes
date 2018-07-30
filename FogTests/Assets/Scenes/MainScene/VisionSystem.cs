using System;
using System.Collections.Generic;
using Gemserk;
using UnityEngine;

public class VisionSystem : MonoBehaviour {

	// TODO: since this is the vision of one player, we could extract part of the structure that represents 
	// the player vision and update each one depending on that, and the texture only updates if it is the 
	// selected players, so the final color depends on the list of current selected players.

	public struct VisionField
	{
		// vision value, > 1 is visible by player.
		public short value;
		public short groundLevel;
		
		// we could have players here, value for each player
		// where player total is a constant
	}

	public int width = 128;
	public int height = 128;

	public int totalPlayers = 2;
	public int currentPlayer = 0;

	public bool raycastEnabled = true;

	[SerializeField]
	protected VisionTexture _visionTexture;
	
	[SerializeField]
	protected VisionCamera _visionCamera;

	[SerializeField]
	protected bool _updateDisabled;

	[SerializeField]
	protected float _updateTotal;

	private VisionField[][] _visionMatrix;

	private float _updateCurrent;
	
	private readonly List<Vision> _visions = new List<Vision>();

	private readonly List<Vision> _addedVisions = new List<Vision>();
	private readonly List<Vision> _removedVisions = new List<Vision>();

	private readonly List<Visible> _visibles = new List<Visible>();
	
	private Vector2 _localScale;

	[SerializeField]
	protected bool _alwaysUpdate;

	private bool _dirty;

	private int _layerVisible;
	private int _layerHidden;

	private int _layerObstacles;
	private readonly RaycastHit2D[] _raycastHit = new RaycastHit2D[1];
	private ContactFilter2D _contactFilter;

	private void Start()
    {
	    // update on first frame
	    _updateCurrent = _updateTotal;

	    _localScale = _visionCamera.GetScale(width, height);
	   
	    _visionTexture.Create(width, height, _localScale);
	    
	    _visionMatrix = new VisionField[totalPlayers][];

	    for (var j = 0; j < totalPlayers; j++)
	    {
		    _visionMatrix[j] = new VisionField[width * height];
		    for (var i = 0; i < width * height; i++)
		    {
			    _visionMatrix[j][i] = new VisionField
			    {
				    value = 0,
				    groundLevel = 0
			    };
		    }
	    }

	    // _localScale = transform.localScale;

	    _layerVisible = LayerMask.NameToLayer("Default");
	    _layerHidden = LayerMask.NameToLayer("Hidden");

	    _layerObstacles = Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("VisionGroundObstacle"));
	    
	    _contactFilter = new ContactFilter2D()
		    .NoFilter();
	    _contactFilter.SetLayerMask(_layerObstacles);
	    
	    // register static ground configuration...

	    var obstacles = FindObjectsOfType<VisionObstacle>();
	    foreach (var obstacle in obstacles)
	    {
		    RegisterObstacle(obstacle);
	    }
    }

	private VisionPosition GetMatrixPosition(Vector2 p)
	{
		var w = (float) width;
		var h = (float) height;

		var i = Mathf.RoundToInt(p.x / _localScale.x + w * 0.5f);
		var j = Mathf.RoundToInt(p.y / _localScale.y + h * 0.5f);

		return new VisionPosition
		{
			x = Math.Max(0, Math.Min(i, width - 1)),
			y = Math.Max(0, Math.Min(j, height - 1))
		};
	}

	private Vector2 GetWorldPosition(int i, int j)
	{
		var w = (float) width;
		var h = (float) height;

		var x = (i - w * 0.5f) * _localScale.x;
		var y = (j - h * 0.5f) * _localScale.y;

		return new Vector2(x, y);
	}

	private void UpdateVision(VisionPosition mp, float visionRange, int player, int groundLevel, short visionValue)
	{
		var visionPosition = GetWorldPosition(mp.x, mp.y);
		
		var currentRowSize = 0;
		var currentColSize = 0;
		
		var rangeSqr = visionRange * visionRange;
		
		var visionWidth = Mathf.RoundToInt(visionRange / _localScale.x);
		var visionHeight = Mathf.RoundToInt(visionRange / _localScale.y);

		var maxColSize = visionWidth;
		var maxRowSize = visionHeight;

		while (currentRowSize != maxRowSize && currentColSize != maxColSize)
		{
			var x = -currentColSize;
			var y = -currentRowSize;
			
			var dx = 1;
			var dy = 0;

			while (true)
			{
				// check current
				var mx = mp.x + x;
				var my = mp.y + y;
				
				var p = GetWorldPosition(mx, my);
				
				var diff = p - visionPosition;
				
				var index = mx + my * width;
				
				if (mx >= 0 && mx < width && my >= 0 && my < height)
				{
					// check if rect to vision center is not blocked

					// var d = diff.normalized;
					
					if (diff.sqrMagnitude < rangeSqr)
					{
//						var raycastCount = Physics2D.Linecast(p, visionPosition, _contactFilter, _raycastHit);
//
//						var blocked = false;
//
//						if (raycastCount > 0)
//						{
//							var raycastCollider = _raycastHit[0].collider;
//							blocked = raycastCollider != null;
//
//							if (blocked)
//							{
//								var obstacle = raycastCollider.GetComponent(typeof(VisionObstacle)) as VisionObstacle;
//								if (obstacle != null)
//									blocked = obstacle.groundLevel >= groundLevel;
//							}
//						}
						var blocked = false;
						
						if (raycastEnabled)
						{
							var raycast = Physics2D.Linecast(p, visionPosition, _layerObstacles);
							var raycastCollider = raycast.collider;

							var obstacle = raycastCollider == null
								? null
								: raycastCollider.GetComponent<VisionObstacle>();

							if (obstacle != null)
								blocked = obstacle.groundLevel >= groundLevel;

						}
						
						if (!blocked)
						{
							// init to +1 first time to mark it as previously visited
							if (_visionMatrix[player][index].value == 0)
								_visionMatrix[player][index].value++;

							_visionMatrix[player][index].value += visionValue;
						}
					}
				}

				if (x + dx > currentColSize)
				{
					dx = 0;
					dy = 1;
				}

				if (y + dy > currentRowSize)
				{
					dx = -1;
					dy = 0;
				}

				if (x + dx < -currentColSize)
				{
					dx = 0;
					dy = -1;
				}

				if (y + dy < -currentRowSize)
				{
					// completed the cycle
					break;
				}
				
				x += dx;
				y += dy;
			}
			
			if (currentRowSize < maxRowSize)
				currentRowSize++;
			
			if (currentColSize < maxColSize)
				currentColSize++;
		}
	}

	private void Update()
	{
		if (_updateDisabled) 
			return;

		_dirty = false;
		
		// _localScale = transform.localScale;

		ProcessPendingVisions();

		_updateCurrent += Time.deltaTime;

		if (_updateCurrent >= _updateTotal)
		{
			_updateCurrent = 0;
			
			foreach (var vision in _visions)
			{	
				vision.position = GetMatrixPosition(vision.worldPosition);
				
				if (vision.position.x == vision.previousPosition.x &&
				    vision.position.y == vision.previousPosition.y &&
				    vision.range == vision.previousRange && 
				    vision.player == vision.previousPlayer && 
				    vision.groundLevel == vision.previousGroundLevel)
					continue;
			
				UpdateVision(vision.previousPosition, vision.previousRange, vision.previousPlayer, vision.previousGroundLevel, -1);
				UpdateVision(vision.position, vision.range, vision.player, vision.groundLevel, 1);
			
				vision.UpdateCachedPosition();
				_dirty = true;
			}
		}

		if (_dirty || _alwaysUpdate)
		{
			_visionTexture.UpdateTexture(_visionMatrix[currentPlayer]);
		}
		
		// update visibles...

		// if not dirty and visible didnt move nor change its visible bounds
		// then don't update
		
		for (var i = 0; i < _visibles.Count; i++)
		{
			var visible = _visibles[i];
			
			var halfwidth = Mathf.CeilToInt(visible.bounds.x * 0.5f / _localScale.x);
			var halfheight = Mathf.CeilToInt(visible.bounds.y * 0.5f / _localScale.y);
			
			visible.matrixPosition = GetMatrixPosition(visible.worldPosition);

			var isVisible = false;

			var colStart = Math.Max(visible.matrixPosition.x - halfwidth, 0);
			var colEnd = Math.Min(visible.matrixPosition.x + halfwidth, width - 1);

			var rowStart = Math.Max(visible.matrixPosition.y - halfheight, 0);
			var rowEnd = Math.Min(visible.matrixPosition.y + halfheight, height - 1);

//			var colStart = visible.matrixPosition[0] - halfwidth;
//			var colEnd = visible.matrixPosition[0] + halfwidth;
//
//			var rowStart = visible.matrixPosition[1] - halfheight;
//			var rowEnd = visible.matrixPosition[1] + halfheight;

			for (var j = colStart; !isVisible && j <= colEnd; j++)
			{
				for (var k = rowStart; !isVisible && k <= rowEnd; k++)
				{
					// change to retuurn 0 if outside matrix instead of fixing to always inside matrix.
//					if (j < 0 || j >= width || k < 0 || k >= height)
//						continue;
					isVisible = _visionMatrix[currentPlayer][j + k * width].value > 1;
				}
			}
			
			visible.visible = isVisible;
			visible.gameObject.SetLayerRecursive(isVisible ? _layerVisible : _layerHidden);
		}
	}

	private void ProcessPendingVisions()
	{
		foreach (var vision in _addedVisions)
		{
			_visions.Add(vision);
			
			vision.position = GetMatrixPosition(vision.worldPosition);
			UpdateVision(vision.position, vision.range, vision.player, vision.groundLevel, 1);
			vision.UpdateCachedPosition();
			
			_dirty = true;
		}

		_addedVisions.Clear();

		foreach (var vision in _removedVisions)
		{
			_visions.Remove(vision);
//			GetMatrixPosition(vision.position, vision.matrixPosition);
			UpdateVision(vision.previousPosition, vision.previousRange, vision.previousPlayer, vision.previousGroundLevel, -1);

			_dirty = true;
		}

		_removedVisions.Clear();
	}

	public void Register(Vision vision)
	{
		_addedVisions.Add(vision);
	}

	public void Unregister(Vision vision)
	{
		_removedVisions.Add(vision);
	}

	public void AddVisible(Visible visible)
	{
		_visibles.Add(visible);
	}

	public void RemoveVisible(Visible visible)
	{
		_visibles.Remove(visible);
	}

	public void RegisterObstacle(VisionObstacle obstacle)
	{
		// TODO: obstacles should be player independant
		
		for (var j = 0; j < totalPlayers; j++)
		{
			for (var i = 0; i < width; i++)
			{
				for (var k = 0; k < height; k++)
				{
					var p = GetWorldPosition(i, k);
					var currentLevel = _visionMatrix[j][i + k * width].groundLevel;
					
					if (currentLevel == 0)
						currentLevel = obstacle.GetGroundLevel(p);

					_visionMatrix[j][i + k * width].groundLevel = currentLevel;
				}
			}
		}		
	}

//	public void UnregisterObstacle(VisionObstacle obstacle)
//	{
//		throw new NotImplementedException();
//	}
}
