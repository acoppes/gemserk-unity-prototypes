using System.Collections.Generic;
using UnityEngine;

public class TextureTest : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public int width = 128;
	public int height = 128;

	Color[] _colors;

	// public int[] vars = new int[5];

	Texture2D _texture;

	// Vision[] _visions;

	public bool testVision = false;

	[SerializeField]
	protected float _updateTotal;

	private float _updateCurrent;
	
	[SerializeField]
	protected TextureFormat _textureFormat;
	
	private readonly List<IVision> _visions = new List<IVision>();

    void Start()
    {
	    _updateCurrent = 0;
	    
		// _visions = FindObjectsOfType<Vision>();

		_texture =  new Texture2D(width, height, _textureFormat, false, false);
		_texture.filterMode = FilterMode.Point;
		_texture.wrapMode = TextureWrapMode.Clamp;

		_colors = new Color[width * height];
		RegenerateColors();

		spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
    }

	void RegenerateColors()
	{
		var blackColor = new Color(0, 0, 0, 1.0f);

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				var color = blackColor;
				_colors[(i * width) + j] = blackColor;				
			}
		}

		_texture.SetPixels(_colors);
		_texture.Apply();
	}

	Vector2 GetWorldPosition(float i, float j)
	{
		var w = (float) width;
		var h = (float) height;

		float x = (i - w * 0.5f) * transform.localScale.x;
		float y = (j - h * 0.5f) * transform.localScale.y;

		// float x = j * transform.localScale.x - width * transform.localScale.x * 0.5f;
		// float y = (i * width) * transform.localScale.y - height * transform.localScale.y * 0.5f;

		return new Vector2(x, y);
	}

	private void UpdateVision()
	{
		var greyColor = new Color(0.5f, 0, 0, 1.0f);
		var whiteColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				var visionFound = false;

				var position = GetWorldPosition(j, i);

				foreach (var vision in _visions)
				{
					if (vision.InRange(position.x, position.y)) {
						_colors[(i * width) + j] = whiteColor;
						visionFound = true;				
						break;
					}	
				}

				if (!visionFound) {
					var currentColor = _colors[(i * width) + j];

					if (currentColor == whiteColor) 
						_colors[(i * width) + j] = greyColor;
				}
			}
		}

		_texture.SetPixels(_colors);
		_texture.Apply();
	}

	private void Update()
	{
		// RegenerateColors(vars[0], vars[1], vars[2], vars[3], vars[4]);
		if (!testVision) 
			return;

		_updateCurrent += Time.deltaTime;

		if (_updateCurrent < _updateTotal)
			return;

		_updateCurrent = 0;
		
		UpdateVision();
		
		// foreach vision check if it was modified (position and range)
	}

	public void Register(IVision vision)
	{
		_visions.Add(vision);
		// update vision matrix
	}

	public void Unregister(IVision vision)
	{
		_visions.Remove(vision);
	}

}
