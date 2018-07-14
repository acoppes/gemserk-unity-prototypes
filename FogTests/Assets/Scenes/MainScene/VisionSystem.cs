using System.Collections.Generic;
using UnityEngine;

public class VisionSystem : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public int width = 128;
	public int height = 128;

	Color[] _colors;

	private int[] _visionMatrix;

	Texture2D _texture;

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

	private Vector2 _localScale;
	
	[SerializeField]
	protected Color _greyColor = new Color(0.5f, 0, 0, 1.0f);
	[SerializeField]
	protected Color _whiteColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	[SerializeField]
	protected Color _errorColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);

	protected bool _dirty;
	
	private void Start()
    {
	    // update on first frame
	    _updateCurrent = _updateTotal;
	    
		// _visions = FindObjectsOfType<Vision>();

		_texture =  new Texture2D(width, height, _textureFormat, false, false);
		_texture.filterMode = FilterMode.Point;
		_texture.wrapMode = TextureWrapMode.Clamp;

		_colors = new Color[width * height];
	    _visionMatrix = new int[width * height];
	    
		ResetVision();

		spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);

	    _localScale = transform.localScale;
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

//		var x = (i - w * 0.5f) * _localScale.x;
//		var y = (j - h * 0.5f) * _localScale.y;

		position[0] = Mathf.RoundToInt(p.x / _localScale.x + w * 0.5f);
		position[1] = Mathf.RoundToInt(p.y / _localScale.y + h * 0.5f);
	}

	private Vector2 GetWorldPosition(int i, int j)
	{
		var w = (float) width;
		var h = (float) height;

		var x = (i - w * 0.5f) * _localScale.x;
		var y = (j - h * 0.5f) * _localScale.y;

		// float x = j * transform.localScale.x - width * transform.localScale.x * 0.5f;
		// float y = (i * width) * transform.localScale.y - height * transform.localScale.y * 0.5f;

		return new Vector2(x, y);
	}

	private void UpdateVision(int[] matrixPosition, float visionRange, int visionValue)
	{
		// TODO: iterate only in range pixels (now it is iterating in all the matrix)

		var visionPosition = GetWorldPosition(matrixPosition[0], matrixPosition[1]);

		var visionHeight = Mathf.RoundToInt(visionRange / _localScale.y);
		var visionWidth = Mathf.RoundToInt(visionRange / _localScale.x);

		var rowStart = Mathf.Max(matrixPosition[1] - visionHeight, 0);
		var rowEnd = Mathf.Min(matrixPosition[1] + visionHeight, height);

		var columnStart = Mathf.Max(matrixPosition[0] - visionWidth, 0);
		var columnEnd = Mathf.Min(matrixPosition[0] + visionWidth, width);

		var rangeSqr = visionRange * visionRange;
		
		for (var i = rowStart; i < rowEnd; i++)
		{
			for (var j = columnStart; j < columnEnd; j++)
			{
				var position = GetWorldPosition(j, i);

				var diff = position - visionPosition;
				
				if (diff.sqrMagnitude < rangeSqr)
				{
					var index = (i * width) + j;

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

		if (_dirty)
			UpdateTexture();
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

}
