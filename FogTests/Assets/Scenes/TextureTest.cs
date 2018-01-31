using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTest : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public int width = 128;
	public int height = 128;

	Color32[] _colors;

	public int[] vars = new int[5];

	Texture2D _texture;

    void Start()
    {
		_texture =  new Texture2D(width, height, TextureFormat.RGBA32, false, true);

		_colors = new Color32[width * height];
		RegenerateColors(vars[0], vars[1], vars[2], vars[3], vars[4]);

		spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
    }

	void RegenerateColors(int x, int y, int w, int h, int border)
	{
		var blackColor = new Color32(0, 0, 0, 255);
		var greyColor = new Color32(128, 0, 0, 0);
		var whiteColor = new Color32(255, 255, 255, 255);

		int minx = x - w;
		int maxx = x + w;

		int miny = y - h;
		int maxy = y + h;

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				var color = blackColor;
				if (i > miny && i < maxy && j > minx  && j < maxx)
					color = greyColor;
				if (i > miny + border && i < maxy - border && j > minx + border && j < maxx - border)
					color = whiteColor;
				_colors[(i * width) + j] = color;				
			}
		}

		_texture.SetPixels32(_colors);
		_texture.Apply();
	}

	void FixedUpdate() {
		RegenerateColors(vars[0], vars[1], vars[2], vars[3], vars[4]);
	}

}
