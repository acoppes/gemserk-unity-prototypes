using System;
using System.Collections;
using System.Collections.Generic;
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

	private VisionSystem.VisionField[] _visionMatrix;
	
	// Use this for initialization
	private void Start () {
		_localScale = _visionCamera.GetScale(_width, _height);
		_visionTexture.Create(_width, _height, _localScale);
		
		_visionMatrix = new VisionSystem.VisionField[_width * _height];
		for (var i = 0; i < _width * _height; i++)
		{
			_visionMatrix[i] = new VisionSystem.VisionField
			{
				value = 0,
				groundLevel = 0
			};
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
//				SetPixel(i, y0 + y);
//				SetPixel(i, y0 - y);

				var i0 = i + (y0 + y) * _width;
				var i1 = i + (y0 - y) * _width;
				
				_visionMatrix[i0].value = 2;
				_visionMatrix[i1].value = 2;
			}
			for (int i = x0 - y; i <= x0 + y; i++)
			{
//				SetPixel(i, y0 + x);
//				SetPixel(i, y0 - x);
				
				var i0 = i + (y0 + x) * _width;
				var i1 = i + (y0 - x) * _width;

				_visionMatrix[i0].value = 2;
				_visionMatrix[i1].value = 2;
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
		for (var i = 0; i < _width * _height; i++)
		{
			_visionMatrix[i].value = 0;
		}
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
		
		_visionTexture.UpdateTexture(_visionMatrix);
	}
	
}
