using System;
using System.Collections.Generic;
using Gemserk;
using Gemserk.Vision;
using UnityEngine;
using UnityEngine.Profiling;


public class VisionSystem : MonoBehaviour {

	// TODO: since this is the vision of one player, we could extract part of the structure that represents 
	// the player vision and update each one depending on that, and the texture only updates if it is the 
	// selected players, so the final color depends on the list of current selected players.

	public struct VisionMatrix
	{
		public int width;
		public int height;

		public int[] values;
		private short[] ground;
		private int[] visited;
		
		// TODO: move player value to vision matrix
		
		public void Init(int width, int height, int value, short groundLevel)
		{
			this.width = width;
			this.height = height;

			var length = width * height;
			
			values = new int[length];
			ground = new short[length];
			visited = new int[length];
			
			Clear(value, groundLevel);
		}

		public bool IsInside(int i, int j)
		{
//			var index = i + j * width;
			return i >= 0 && i < width && j >= 0 && j < height;
		}

		public void SetVisible(int playerFlags, int i, int j)
		{
			visited[i + j * width] |= playerFlags;
			values[i + j * width] |= playerFlags;
		}

		public bool IsVisible(int playerFlags, int i, int j)
		{
			return (values[i + j * width] & playerFlags) > 0;
		}
		
		public bool IsVisible(int playerFlags, int i)
		{
			return (values[i] & playerFlags) > 0;
		}
		
		public bool WasVisible(int playerFlags, int i)
		{
			return (visited[i] & playerFlags) > 0;
		}

		public short GetGround(int i, int j)
		{
			return ground[i + j * width];
		}
		
		public short GetGround(int i)
		{
			return ground[i];
		}
		
		public void SetGround(int i, int j, short ground)
		{
			this.ground[i + j * width] = ground;
		}

		public void Clear(int value, short groundLevel)
		{
			for (var i = 0; i < width * height; i++)
			{
				visited[i] = 0;
				ground[i] = groundLevel;
				values[i] = value;
			}
		}
		
		public void ClearValues()
		{
			Array.Clear(values, 0, values.Length);
		}

		public void Clear()
		{
			Array.Clear(values, 0, values.Length);
			Array.Clear(visited, 0, visited.Length);
		}

	}

	public int width = 128;
	public int height = 128;

	public int totalPlayers = 2;
	
	// flag...
	public int _activePlayers = 1;

	public bool raycastEnabled = true;

	[SerializeField]
	protected VisionTexture _visionTexture;
	
	[SerializeField]
	protected VisionCamera _visionCamera;
	
	[SerializeField]
	protected VisionTerrainTexture _visionTerrain;

	[SerializeField]
	protected bool _updateDisabled;
	
	[SerializeField]
	protected float _updateTotal;

	private VisionMatrix _visionMatrix;

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

	private static CachedIntAbsoluteValues cachedAbsoluteValues;

	[SerializeField]
	public bool updateMethod;
	
	private void Start()
    {
	    // update on first frame
	    _updateCurrent = _updateTotal;

	    _localScale = _visionCamera.GetScale(width, height);
	   
	    _visionTexture.Create(width, height, _localScale);

	    if (_visionTerrain != null)
	    {
		    _visionTerrain.Create(width, height, _localScale);
	    }
	    
	    _visionMatrix = new VisionMatrix();
	    _visionMatrix.Init(width, height, 0, 0);

//	    for (var j = 0; j < totalPlayers; j++)
//	    {
//		    _visionMatrix[j].Init(width, height, 0, 0);
//	    }

	    // _localScale = transform.localScale;

	    _layerVisible = LayerMask.NameToLayer("Default");
	    _layerHidden = LayerMask.NameToLayer("Hidden");

	    // register static ground configuration...

	    var obstacles = FindObjectsOfType<VisionObstacle>();
	    foreach (var obstacle in obstacles)
	    {
		    RegisterObstacle(obstacle);
	    }

	    cachedAbsoluteValues.Init(Math.Max(width, height));
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

	private static bool IsBlocked(VisionMatrix visionMatrix, short groundLevel, int x0, int y0, int x1, int y1)
	{
		Profiler.BeginSample("IsBlocked");

		int dx = cachedAbsoluteValues.Abs(x1 - x0);
		int dy = cachedAbsoluteValues.Abs(y1 - y0);
		
		int sx = x0 < x1 ? 1 : -1;
		int sy = y0 < y1 ? 1 : -1;

		int err = (dx > dy ? dx : -dy) / 2;
		int e2;

		var blocked = false;
		
		for (;;)
		{
//			if (_currentBlocked[x1 - x0 + 50, y1 - y0 + 50])
//			{
//				blocked = true;
//				break;
//			}
			
			var ground = visionMatrix.GetGround(x0, y0);

			// var visionField = visionMatrix.vision[x0 + y0 * visionMatrix.width];

			if (ground > groundLevel)
			{
				blocked = true;
				break;
			}
			
			if (x0 == x1 && y0 == y1)
				break;

			e2 = err;
			
			if (e2 > -dx)
			{
				err -= dy;
				x0 += sx;
			}

			if (e2 >= dy) 
				continue;
			
			err += dx;
			y0 += sy;
		}

		Profiler.EndSample();
		return blocked;
	}
	
	private void DrawPixel(VisionMatrix visionMatrix, int player, int x0, int y0, int x, int y, short groundLevel)
	{
		if (!visionMatrix.IsInside(x, y))
			return;
		
		var blocked = raycastEnabled && IsBlocked(visionMatrix, groundLevel, x, y, x0, y0);

		if (blocked)
		{
//			_currentBlocked[x0 - x + 50, y0 - y + 50] = true;
			return;
		}
		
//		_currentBlocked[x0 - x + 50, y0 - y + 50] = false;
		// TODO: think a way of improving this..
		// if (_visionMatrix.GetValue(player, x, y) == 0)
		
		visionMatrix.SetVisible(player, x, y);
	}


	private void UpdateVision(VisionPosition mp, float visionRange, int player, short groundLevel)
	{
		if (!updateMethod)
		{
			UpdateVision1(mp, visionRange, player, groundLevel);
		}
		else
		{
			UpdateVision2(mp, visionRange, player, groundLevel);
		}
	}

	// max matrix size, if vision range > max, then cant use the cached matrix
//	private static bool[,] _currentBlocked = new bool[100, 100];

	private void UpdateVision2(VisionPosition mp, float visionRange, int player, short groundLevel)
	{
		int radius = Mathf.CeilToInt(visionRange / _localScale.x);
		int x0 = mp.x;
		int y0 = mp.y;
		
		int x = radius;
		int y = 0;
		int xChange = 1 - (radius << 1);
		int yChange = 0;
		int radiusError = 0;
		
		// use a cached sub matrix for blocked pixels
//		Array.Clear(_currentBlocked, 0, _currentBlocked.Length);
		
		while (x >= y)
		{
			for (var i = x0 - x; i <= x0 + x; i++)
			{
				DrawPixel(_visionMatrix, player, x0, y0, i, y0 + y, groundLevel);
				DrawPixel(_visionMatrix, player, x0, y0, i, y0 - y, groundLevel);
			}
			for (var i = x0 - y; i <= x0 + y; i++)
			{
				DrawPixel(_visionMatrix, player, x0, y0, i, y0 + x, groundLevel);
				DrawPixel(_visionMatrix, player, x0, y0, i, y0 - x, groundLevel);
			}

			y++;
			radiusError += yChange;
			yChange += 2;
			
			if (((radiusError << 1) + xChange) > 0)
			{
				x--;
				radiusError += xChange;
				xChange += 2;
			}
		}
		
	}

	private void UpdateVision1(VisionPosition mp, float visionRange, int player, short groundLevel)
	{
		var visionPosition = GetWorldPosition(mp.x, mp.y);
		
		var currentRowSize = 0;
		var currentColSize = 0;
		
		var rangeSqr = visionRange * visionRange;
		
		var visionWidth = Mathf.RoundToInt(visionRange / _localScale.x);
		var visionHeight = Mathf.RoundToInt(visionRange / _localScale.y);

		var maxColSize = visionWidth;
		var maxRowSize = visionHeight;

//		var visionMatrix = _visionMatrix[player];

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
				
				if (mx >= 0 && mx < width && my >= 0 && my < height)
				{
					// check if rect to vision center is not blocked

					// var d = diff.normalized;
					
					if (diff.sqrMagnitude < rangeSqr)
					{
						var blocked = raycastEnabled && IsBlocked(_visionMatrix, groundLevel, mx, my, mp.x, mp.y);
						
						if (!blocked)
						{
//							throw new NotImplementedException("new implementation doesnt support this method");
							// if (_visionMatrix.GetValue(player, mx, my) == 0)
							_visionMatrix.SetVisible(player, mx, my);
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

	public void Clear()
	{
		_visionMatrix.Clear();
	}

	private void FixedUpdate()
	{
		if (_updateDisabled) 
			return;

		if (_visionTerrain != null)
		{
			_visionTerrain.UpdateTexture(_visionMatrix);
		}
		
		_dirty = false;
		
		// _localScale = transform.localScale;

		ProcessPendingVisions();

		_updateCurrent += Time.deltaTime;

		if (_updateCurrent >= _updateTotal)
		{
			Profiler.BeginSample("VisionUpdate");

			_updateCurrent = 0;

			_visionMatrix.ClearValues();
			
			foreach (var vision in _visions)
			{	
				vision.position = GetMatrixPosition(vision.worldPosition);
				
				UpdateVision(vision.position, vision.range, vision.player, vision.groundLevel);

				_dirty = true;
			}
			
			Profiler.EndSample();
		}

		if (_dirty || _alwaysUpdate)
		{
			_visionTexture.UpdateTexture(_visionMatrix, _activePlayers);
		}
		
		// update visibles...

		// if not dirty and visible didnt move nor change its visible bounds
		// then don't update
		
		Profiler.BeginSample("Visibles");
		
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

			for (var j = colStart; !isVisible && j <= colEnd; j++)
			{
				for (var k = rowStart; !isVisible && k <= rowEnd; k++)
				{
					isVisible = _visionMatrix.IsVisible(_activePlayers, j, k);
				}
			}
			
			visible.visible = isVisible;
			visible.gameObject.SetLayerRecursive(isVisible ? _layerVisible : _layerHidden);
		}
		
		Profiler.EndSample();
	}

	private void ProcessPendingVisions()
	{
		foreach (var vision in _addedVisions)
		{
			_visions.Add(vision);
			
			vision.position = GetMatrixPosition(vision.worldPosition);
			UpdateVision(vision.position, vision.range, vision.player, vision.groundLevel);
			
			_dirty = true;
		}

		_addedVisions.Clear();

		foreach (var vision in _removedVisions)
		{
			_visions.Remove(vision);
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

	private void RegisterObstacle(VisionObstacle obstacle)
	{
		for (var j = 0; j < totalPlayers; j++)
		{
			for (var i = 0; i < width; i++)
			{
				for (var k = 0; k < height; k++)
				{
					var p = GetWorldPosition(i, k);
					var currentLevel = _visionMatrix.GetGround(i, k);
					
					if (currentLevel == 0)
						currentLevel = obstacle.GetGroundLevel(p);

					_visionMatrix.SetGround(i, k, currentLevel);
				}
			}
		}		
	}

//	public void UnregisterObstacle(VisionObstacle obstacle)
//	{
//		throw new NotImplementedException();
//	}
	
	public short GetGroundLevel(Vector3 position)
	{
		var mp = GetMatrixPosition(position);
		return _visionMatrix.GetGround(mp.x, mp.y);
	}
}
