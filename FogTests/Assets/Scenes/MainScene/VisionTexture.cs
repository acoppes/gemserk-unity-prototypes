using System;
using UnityEngine;

public class VisionTexture : MonoBehaviour
{
    [NonSerialized]
    private Texture2D _texture;

    [SerializeField]
    protected TextureFormat _textureFormat;

    [SerializeField]
    protected SpriteRenderer _spriteRenderer;
	
    public void Create(int width, int height)
    {
        _texture =  new Texture2D(width, height, _textureFormat, false, false);
        _texture.filterMode = FilterMode.Point;
        _texture.wrapMode = TextureWrapMode.Clamp;
		
        _spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
    }

    public void OnVisionUpdated(Color[] colors)
    {
        _texture.SetPixels(colors);
        _texture.Apply();
    }
}