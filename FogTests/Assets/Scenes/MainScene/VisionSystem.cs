using System;
using System.Collections.Generic;
using Gemserk;
using UnityEngine;

public class VisionSystem : MonoBehaviour {

	// TODO: since this is the vision of one player, we could extract part of the structure that represents 
	// the player vision and update each one depending on that, and the texture only updates if it is the 
	// selected players, so the final color depends on the list of current selected players.
	
	[SerializeField]
	protected SpriteRenderer spriteRenderer;

	public int width = 128;
	public int height = 128;

	private Color[] _colors;

	private int[] _visionMatrix;

	private Texture2D _texture;

	[SerializeField]
	protected bool _updateDisabled;

	[SerializeField]
	protected float _updateTotal;

	private float _updateCurrent;
	
	[SerializeField]
	protected TextureFormat _textureFormat;
	
	private readonly List<Vision> _visions = new List<Vision>();

	private readonly List<Vision> _addedVisions = new List<Vision>();
	private readonly List<Vision> _removedVisions = new List<Vision>();

	private readonly List<Visible> _visibles = new List<Visible>();
	
	private Vector2 _localScale;
	
	[SerializeField]
	protected Color _greyColor = new Color(0.5f, 0, 0, 1.0f);
	[SerializeField]
	protected Color _whiteColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	[SerializeField]
	protected Color _errorColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);

	[SerializeField]
	protected bool _alwaysUpdate;

	private bool _dirty;

	private int _layerVisible;
	private int _layerHidden;

	[SerializeField]
	protected PolygonCollider2D _testObstacle;
	
	private void Start()
    {
	    // update on first frame
	    _updateCurrent = _updateTotal;

		_texture =  new Texture2D(width, height, _textureFormat, false, false);
		_texture.filterMode = FilterMode.Point;
		_texture.wrapMode = TextureWrapMode.Clamp;

		_colors = new Color[width * height];
	    _visionMatrix = new int[width * height];
	    
		ResetVision();

		spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);

	    _localScale = transform.localScale;

	    _layerVisible = LayerMask.NameToLayer("Default");
	    _layerHidden = LayerMask.NameToLayer("Hidden");
    }

	private void ResetVision()
	{
		var blackColor = new Color(0, 0, 0, 1.0f);

		for (var i = 0; i < width * height; i++)
		{
			_colors[i] = blackColor;
			_visionMatrix[i] = 0;
		}

		_texture.SetPixels(_colors);
		_texture.Apply();
	}

	private void GetMatrixPosition(Vector2 p, int[] position)
	{
		var w = (float) width;
		var h = (float) height;

		var i = Mathf.RoundToInt(p.x / _localScale.x + w * 0.5f);
		var j = Mathf.RoundToInt(p.y / _localScale.y + h * 0.5f);

		position[0] = Math.Max(0, Math.Min(i, width - 1));
		position[1] = Math.Max(0, Math.Min(j, height - 1));
	}

	private Vector2 GetWorldPosition(int i, int j)
	{
		var w = (float) width;
		var h = (float) height;

		var x = (i - w * 0.5f) * _localScale.x;
		var y = (j - h * 0.5f) * _localScale.y;

		return new Vector2(x, y);
	}

	private void UpdateVision(int[] matrixPosition, float visionRange, int visionValue)
	{
		var visionPosition = GetWorldPosition(matrixPosition[0], matrixPosition[1]);

		var visionHeight = Mathf.RoundToInt(visionRange / _localScale.y);
		var visionWidth = Mathf.RoundToInt(visionRange / _localScale.x);

		var rowStart = Mathf.Max(matrixPosition[1] - visionHeight, 0);
		var rowEnd = Mathf.Min(matrixPosition[1] + visionHeight + 2, height - 1);

		var columnStart = Mathf.Max(matrixPosition[0] - visionWidth - 1, 0);
		var columnEnd = Mathf.Min(matrixPosition[0] + visionWidth + 1, width - 1);

		var rangeSqr = visionRange * visionRange;
		
		for (var i = rowStart; i <= rowEnd; i++)
		{
			for (var j = columnStart; j <= columnEnd; j++)
			{
				var position = GetWorldPosition(j, i);
				var index = (i * width) + j;

				if (_testObstacle.OverlapPoint(position))
				{
					_visionMatrix[index] = -10;
				}

				var diff = position - visionPosition;
				
				if (diff.sqrMagnitude < rangeSqr)
				{

					// init to +1 first time to mark it as previously visited
					if (_visionMatrix[index] == 0)
						_visionMatrix[index]++;
					
					_visionMatrix[index] += visionValue;
				}
			}
		}
	}

	private void Update()
	{
		if (_updateDisabled) 
			return;

		_dirty = false;
		
		_localScale = transform.localScale;

		ProcessPendingVisions();

		_updateCurrent += Time.deltaTime;

		if (_updateCurrent >= _updateTotal)
		{
			_updateCurrent = 0;
			
			foreach (var vision in _visions)
			{	
				GetMatrixPosition(vision.worldPosition, vision.currentPosition);
				
				if (vision.currentPosition[0] == vision.previousPosition[0] &&
				    vision.currentPosition[1] == vision.previousPosition[1] &&
				    vision.currentRange == vision.previousRange)
					continue;
			
				UpdateVision(vision.previousPosition, vision.previousRange, -1);
				UpdateVision(vision.currentPosition, vision.currentRange, 1);
			
				vision.UpdateCachedPosition();
				_dirty = true;
			}
		}

		if (_dirty || _alwaysUpdate)
			UpdateTexture();
		
		// update visibles...

		// if not dirty and visible didnt move nor change its visible bounds
		// then don't update
		
		for (var i = 0; i < _visibles.Count; i++)
		{
			var visible = _visibles[i];
			
			var halfwidth = Mathf.CeilToInt(visible.bounds.x * 0.5f / _localScale.x);
			var halfheight = Mathf.CeilToInt(visible.bounds.y * 0.5f / _localScale.y);
			
			GetMatrixPosition(visible.worldPosition, visible.matrixPosition);

			var isVisible = false;

			var colStart = Math.Max(visible.matrixPosition[0] - halfwidth, 0);
			var colEnd = Math.Min(visible.matrixPosition[0] + halfwidth, width - 1);

			var rowStart = Math.Max(visible.matrixPosition[1] - halfheight, 0);
			var rowEnd = Math.Min(visible.matrixPosition[1] + halfheight, height - 1);

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
					isVisible = _visionMatrix[j + k * width] > 1;
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
			
			GetMatrixPosition(vision.worldPosition, vision.currentPosition);
			UpdateVision(vision.currentPosition, vision.currentRange, 1);
			vision.UpdateCachedPosition();
			
			_dirty = true;
		}

		_addedVisions.Clear();

		foreach (var vision in _removedVisions)
		{
			_visions.Remove(vision);
//			GetMatrixPosition(vision.position, vision.matrixPosition);
			UpdateVision(vision.previousPosition, vision.previousRange, -1);

			_dirty = true;
		}

		_removedVisions.Clear();
	}

	private void UpdateTexture()
	{
		for (var i = 0; i < width * height; i++)
		{
			if (_visionMatrix[i] > 1)
			{
				_colors[i] = _whiteColor;
				continue;
			}

			if (_visionMatrix[i] == 1)
			{
				_colors[i] = _greyColor;
				continue;
			}
			
			// this is just for debug reasons
			if (_visionMatrix[i] < 0)
			{
				_colors[i] = _errorColor;
			}
		}
		
		_texture.SetPixels(_colors);
		_texture.Apply();
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
}
