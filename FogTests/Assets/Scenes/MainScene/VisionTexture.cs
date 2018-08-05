using System;
using System.Collections.Generic;
using UnityEngine;

public class VisionTexture : MonoBehaviour
{
    [SerializeField]
    protected TextureFormat _textureFormat;

    [SerializeField]
    protected SpriteRenderer _spriteRenderer;

    [NonSerialized]
    private Texture2D _texture;

    [NonSerialized]
    private Color[] _colors;
    
    [SerializeField]
    protected Color _greyColor = new Color(0.5f, 0, 0, 1.0f);
    
    [SerializeField]
    protected Color _whiteColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    
    [SerializeField]
    protected Color _errorColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);

    [SerializeField] protected Color _startColor = new Color(0, 0, 0, 1.0f);

    [SerializeField] 
    protected Color[] _groundColors = new Color[]
    {
        new Color(0, 0, 0.2f, 1.0f),
        new Color(0, 0, 0.6f, 1.0f),
        new Color(0, 0, 0.8f, 1.0f),
    };

    [SerializeField]
    protected bool _writeGroundColor;

    private int _width;
    private int _height;
	
    public void Create(int width, int height, Vector2 scale)
    {
        _width = width;
        _height = height;
        
        _texture =  new Texture2D(width, height, _textureFormat, false, false);
        _texture.filterMode = FilterMode.Point;
        _texture.wrapMode = TextureWrapMode.Clamp;
		
        _spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
        _spriteRenderer.transform.localScale = scale;
        
        _colors = new Color[width * height];
        
        for (var i = 0; i < width * height; i++)
        {
            _colors[i] = _startColor;
        }
        
        _texture.SetPixels(_colors);
        _texture.Apply();
    }
    
    public void UpdateTexture(VisionSystem.VisionField[] visionMatrix)
    {
        for (var i = 0; i < _width * _height; i++)
        {
            var visionField = visionMatrix[i];
            
            // TODO: constants for visions in vision system.
            _colors[i] = _startColor;

            var value = visionField.value;
            
            if (value > 1)
            {
                _colors[i] = _whiteColor;
            } else if (value == 1)
            {
                _colors[i] = _greyColor;
            } else if (value < 0)
            {
                // for debug, should never be < 0
                _colors[i] = _errorColor;
            }

            if (_writeGroundColor)
            {
                var groundColor = _groundColors[visionField.groundLevel];
                _colors[i] += groundColor;
            }
        }
		
        _texture.SetPixels(_colors);
        _texture.Apply();
    }

}