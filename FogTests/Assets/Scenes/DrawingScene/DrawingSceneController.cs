using System;
using Gemserk.Vision;
using UnityEngine;

public class DrawingSceneController : MonoBehaviour
{
	[SerializeField]
	protected Camera _camera;
	
	[SerializeField]
	protected VisionTexture _visionTexture;
	
	[SerializeField]
	protected VisionCamera _visionCamera;

	[SerializeField]
	protected int _width;
	[SerializeField]
	protected int _height;
	
	private Vector2 _localScale;

	private VisionMatrix _visionMatrix;
	
	// Use this for initialization
	private void Start () {
		_localScale = _visionCamera.GetScale(_width, _height);
		_visionTexture.Create(_width, _height, _localScale);
		
//		_visionMatrix = new VisionSystem.VisionMatrix[_width * _height];
		
		_visionMatrix.Init(_width, _height, 0, 0);
	}

	// TODO: create struct/class for vision matrix with width/height in it.
	private static void DrawPixel(VisionMatrix visionMatrix, int x, int y, int value)
	{
		if (visionMatrix.IsInside(x, y))
		{
			visionMatrix.SetVisible(1, x, y);
		}
	}
	
	void DrawFilledCircle(int x0, int y0, int radius)
	{
		int x = radius;
		int y = 0;
		int xChange = 1 - (radius << 1);
		int yChange = 0;
		int radiusError = 0;

		while (x >= y)
		{
			for (int i = x0 - x; i <= x0 + x; i++)
			{
				var i0 = i + (y0 + y) * _width;
				var i1 = i + (y0 - y) * _width;

				DrawPixel(_visionMatrix, i, y0 + y, 2);
				DrawPixel(_visionMatrix, i, y0 - y, 2);
			}
			for (int i = x0 - y; i <= x0 + y; i++)
			{
				var i0 = i + (y0 + x) * _width;
				var i1 = i + (y0 - x) * _width;

				DrawPixel(_visionMatrix, i, y0 + x, 2);
				DrawPixel(_visionMatrix, i, y0 - x, 2);
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

	[SerializeField]
	protected int _testRadius = 30;
	
	private VisionPosition GetMatrixPosition(Vector2 p)
	{
		var w = (float) _width;
		var h = (float) _height;

		var i = Mathf.RoundToInt(p.x / _localScale.x + w * 0.5f);
		var j = Mathf.RoundToInt(p.y / _localScale.y + h * 0.5f);

		return new VisionPosition
		{
			x = Math.Max(0, Math.Min(i, _width - 1)),
			y = Math.Max(0, Math.Min(j, _height - 1))
		};
	}

	private void ClearMatrix()
	{
		_visionMatrix.Clear(0, 0);
	}
	
	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			var v3 = Input.mousePosition;
			v3 = _camera.ScreenToWorldPoint(v3);

			var mp = GetMatrixPosition(v3);
			
			DrawFilledCircle(mp.x, mp.y, _testRadius);
		}
		
		if (Input.GetMouseButtonUp(1))
		{
			ClearMatrix();
		}
		
		_visionTexture.UpdateTexture(_visionMatrix, 1);
	}
	
}
