using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTest : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public int width = 128;
	public int height = 128;

	Color32[] _colors;

    void Start()
    {
        // Renderer rend = GetComponent<Renderer>();

        // // duplicate the original texture and assign to the material
        // Texture2D texture = Instantiate(rend.material.mainTexture) as Texture2D;
        // rend.material.mainTexture = texture;

        // // colors used to tint the first 3 mip levels
        // Color[] colors = new Color[3];
        // colors[0] = Color.red;
        // colors[1] = Color.green;
        // colors[2] = Color.blue;
        // int mipCount = Mathf.Min(3, texture.mipmapCount);

        // // tint each mip level
        // for (int mip = 0; mip < mipCount; ++mip)
        // {
        //     Color[] cols = texture.GetPixels(mip);
        //     for (int i = 0; i < cols.Length; ++i)
        //     {
        //         cols[i] = Color.Lerp(cols[i], colors[mip], 0.33f);
        //     }
        //     texture.SetPixels(cols, mip);
        // }
        // // actually apply all SetPixels, don't recalculate mip levels
        // texture.Apply(false);


		_colors = new Color32[width * height];

		var blackColor = new Color32(0, 0, 0, 255);
		var whiteColor = new Color32(255, 255, 255, 255);

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				var color = blackColor;
				if (i > 50 && i < 100 && j > 50 && j < 100)
					color = whiteColor;
				_colors[(i * width) + j] = color;				
			}
		}

		var texture =  new Texture2D(width, height, TextureFormat.RGBA32, false);
		texture.SetPixels32(_colors);
		texture.Apply();

		spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
    }

}
