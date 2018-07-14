using System.Collections.Generic;
using UnityEngine;

public class VisionSystem : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public int width = 128;
	public int height = 128;

	Color[] _colors;

	private int[] _visionMatrix;

	Texture2D _texture;

	public bool testVision = false;

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

	
	private void Start()
    {
	    _updateCurrent = 0;
	    
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

	private Vector2 GetWorldPosition(float i, float j)
	{
		var w = (float) width;
		var h = (float) height;

		var x = (i - w * 0.5f) * _localScale.x;
		var y = (j - h * 0.5f) * _localScale.y;

		// float x = j * transform.localScale.x - width * transform.localScale.x * 0.5f;
		// float y = (i * width) * transform.localScale.y - height * transform.localScale.y * 0.5f;

		return new Vector2(x, y);
	}

	private void UpdateVision(Vector2 visionPosition, float visionRange, int visionValue)
	{
		// TODO: iterate only in range pixels (now it is iterating in all the matrix)
		
		for (var i = 0; i < height; i++)
		{
			for (var j = 0; j < width; j++)
			{
				var position = GetWorldPosition(j, i);

				if (Vector2.Distance(position, visionPosition) < visionRange)
				{
					_visionMatrix[(i * width) + j] += visionValue;
				}
			}
		}
	}

	private void Update()
	{
		// RegenerateColors(vars[0], vars[1], vars[2], vars[3], vars[4]);
		if (!testVision) 
			return;
		
		_localScale = transform.localScale;

		ProcessPendingVisions();

		_updateCurrent += Time.deltaTime;

		if (_updateCurrent < _updateTotal)
			return;

		_updateCurrent = 0;
		
		// UpdateVision();
		
		// foreach vision check if it was modified (position and range)
//		var dirty = false;

		foreach (var vision in _visions)
		{
			var visionPosition = vision.position;
			var visionCachedPosition = vision.cachedPosition;
			
			if (Vector2.Distance(visionPosition, visionCachedPosition) < Mathf.Epsilon) 
				continue;
			
			UpdateVision(visionCachedPosition, vision.range, -1);
			UpdateVision(visionPosition, vision.range, 1);
			
			vision.UpdateCachedPosition();
//			dirty = true;
		}

//		if (!dirty) 
//			return;

		UpdateTexture();
	}

	private void ProcessPendingVisions()
	{
		foreach (var vision in _addedVisions)
		{
			_visions.Add(vision);
			UpdateVision(vision.position, vision.range, 1);
			vision.UpdateCachedPosition();
		}

		_addedVisions.Clear();

		foreach (var vision in _removedVisions)
		{
			_visions.Remove(vision);
			UpdateVision(vision.position, vision.range, -1);
		}

		_removedVisions.Clear();
	}

	private void UpdateTexture()
	{
		for (var i = 0; i < width * height; i++)
		{
			if (_visionMatrix[i] > 0)
			{
				_colors[i] = _whiteColor;
				continue;
			}

			var currentColor = _colors[i];

			if (currentColor == _whiteColor)
				_colors[i] = _greyColor;
			
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
