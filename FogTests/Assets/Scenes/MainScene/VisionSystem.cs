using System;
using System.Collections.Generic;
using Gemserk;
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

		public short[] values;
		public short[] ground;
		public bool[] visited;
		
		// TODO: move player value to vision matrix
		
		public void Init(int width, int height, short value, short groundLevel)
		{
			this.width = width;
			this.height = height;

			var length = width * height;
			
			values = new short[length];
			ground = new short[length];
			visited = new bool[length];
			
			Clear(value, groundLevel);
		}

		public bool IsInside(int i, int j)
		{
			var index = i + j * width;
			return index >= 0 && index < values.Length;
		}

		public void SetValue(int i, int j, short value)
		{
			visited[i + j * width] = true;
			values[i + j * width] = value;
		}
		
		public void AddValue(int i, int j, short value)
		{
			visited[i + j * width] = true;
			values[i + j * width] += value;
		}

		public short GetValue(int i, int j)
		{
			return values[i + j * width];
		}

		public short GetGround(int i, int j)
		{
			return ground[i + j * width];
		}
		
		public void SetGround(int i, int j, short ground)
		{
			this.ground[i + j * width] = ground;
		}

		public void Clear(short value, short groundLevel)
		{
			for (var i = 0; i < width * height; i++)
			{
				visited[i] = false;
				ground[i] = groundLevel;
				values[i] = value;
			}
		}
		
		public void ClearValues()
		{
			Array.Clear(values, 0, values.Length);
		}


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
	public bool _resetMatrixEachFrame;

	[SerializeField]
	protected float _updateTotal;

	private VisionMatrix[] _visionMatrixPerPlayer;

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
	    
	    _visionMatrixPerPlayer = new VisionMatrix[totalPlayers];

	    for (var j = 0; j < totalPlayers; j++)
	    {
		    _visionMatrixPerPlayer[j].Init(width, height, 0, 0);
	    }

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

		for (;;)
		{
			var ground = visionMatrix.GetGround(x0, y0);

			// var visionField = visionMatrix.vision[x0 + y0 * visionMatrix.width];

			if (ground > groundLevel)
			{
				Profiler.EndSample();
				return true;
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
		return false;
	}
	
	private void DrawPixel(VisionMatrix visionMatrix, int x0, int y0, int x, int y, short value, short groundLevel)
	{
		if (!visionMatrix.IsInside(x, y))
			return;
		
		var blocked = raycastEnabled && IsBlocked(visionMatrix, groundLevel, x, y, x0, y0);

		if (blocked) 
			return;
		
		visionMatrix.AddValue(x, y, value);
	}


	private void UpdateVision(VisionPosition mp, float visionRange, int player, short groundLevel, short visionValue)
	{
		if (!updateMethod)
		{
			UpdateVision1(mp, visionRange, player, groundLevel, visionValue);
		}
		else
		{
			UpdateVision2(mp, visionRange, player, groundLevel, visionValue);
		}
	}

//	private short[] currentVisionTest = new short[100*100];

	private void UpdateVision2(VisionPosition mp, float visionRange, int player, short groundLevel, short value)
	{
		int radius = Mathf.CeilToInt(visionRange / _localScale.x);
		int x0 = mp.x;
		int y0 = mp.y;
		
		int x = radius;
		int y = 0;
		int xChange = 1 - (radius << 1);
		int yChange = 0;
		int radiusError = 0;

		var visionMatrix = _visionMatrixPerPlayer[player];
		
//		Array.Clear(currentVisionTest, 0, currentVisionTest.Length);
		
		while (x >= y)
		{
			for (var i = x0 - x; i <= x0 + x; i++)
			{
				DrawPixel(visionMatrix, x0, y0, i, y0 + y, value, groundLevel);
				DrawPixel(visionMatrix, x0, y0, i, y0 - y, value, groundLevel);
			}
			for (var i = x0 - y; i <= x0 + y; i++)
			{
				DrawPixel(visionMatrix, x0, y0, i, y0 + x, value, groundLevel);
				DrawPixel(visionMatrix, x0, y0, i, y0 - x, value, groundLevel);
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

	private void UpdateVision1(VisionPosition mp, float visionRange, int player, short groundLevel, short visionValue)
	{
		var visionPosition = GetWorldPosition(mp.x, mp.y);
		
		var currentRowSize = 0;
		var currentColSize = 0;
		
		var rangeSqr = visionRange * visionRange;
		
		var visionWidth = Mathf.RoundToInt(visionRange / _localScale.x);
		var visionHeight = Mathf.RoundToInt(visionRange / _localScale.y);

		var maxColSize = visionWidth;
		var maxRowSize = visionHeight;

		var visionMatrix = _visionMatrixPerPlayer[player];

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
						var blocked = raycastEnabled && IsBlocked(visionMatrix, groundLevel, mx, my, mp.x, mp.y);
						
						if (!blocked)
						{
							visionMatrix.AddValue(mx, my, visionValue);
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

	private void FixedUpdate()
	{
		if (_updateDisabled) 
			return;
		
		_dirty = false;
		
		// _localScale = transform.localScale;

		ProcessPendingVisions();

		_updateCurrent += Time.deltaTime;

		if (_updateCurrent >= _updateTotal)
		{
			Profiler.BeginSample("VisionUpdate");

			_updateCurrent = 0;

			if (_resetMatrixEachFrame)
			{
				foreach (var _visionMatrix in _visionMatrixPerPlayer)
				{
					_visionMatrix.ClearValues();
				}
			}
			
			foreach (var vision in _visions)
			{	
				vision.position = GetMatrixPosition(vision.worldPosition);
				
				if (!_resetMatrixEachFrame && vision.position.x == vision.previousPosition.x &&
				    vision.position.y == vision.previousPosition.y &&
				    vision.range == vision.previousRange && 
				    vision.player == vision.previousPlayer && 
				    vision.groundLevel == vision.previousGroundLevel)
					continue;
			
				if (!_resetMatrixEachFrame)
					UpdateVision(vision.previousPosition, vision.previousRange, vision.previousPlayer, vision.previousGroundLevel, -1);
				
				UpdateVision(vision.position, vision.range, vision.player, vision.groundLevel, 1);
			
				vision.UpdateCachedPosition();
				_dirty = true;
			}
			
			Profiler.EndSample();
		}

		if (_dirty || _alwaysUpdate)
		{
			_visionTexture.UpdateTexture(_visionMatrixPerPlayer[currentPlayer]);
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
					isVisible = _visionMatrixPerPlayer[currentPlayer].GetValue(j, k) > 1;
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
			UpdateVision(vision.position, vision.range, vision.player, vision.groundLevel, 1);
			vision.UpdateCachedPosition();
			
			_dirty = true;
		}

		_addedVisions.Clear();

		foreach (var vision in _removedVisions)
		{
			_visions.Remove(vision);
//			GetMatrixPosition(vision.position, vision.matrixPosition);
			if (!_resetMatrixEachFrame)
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

	private void RegisterObstacle(VisionObstacle obstacle)
	{
		// TODO: obstacles should be player independant
		
		for (var j = 0; j < totalPlayers; j++)
		{
			for (var i = 0; i < width; i++)
			{
				for (var k = 0; k < height; k++)
				{
					var p = GetWorldPosition(i, k);
					var currentLevel = _visionMatrixPerPlayer[j].GetGround(i, k);
					
					if (currentLevel == 0)
						currentLevel = obstacle.GetGroundLevel(p);

					_visionMatrixPerPlayer[j].SetGround(i, k, currentLevel);
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
		return _visionMatrixPerPlayer[currentPlayer].GetGround(mp.x, mp.y);
	}
}
