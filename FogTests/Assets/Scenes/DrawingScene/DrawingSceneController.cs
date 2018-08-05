using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingSceneController : MonoBehaviour
{

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
				value = 1,
				groundLevel = 0
			};
		}
	}

	private void Update()
	{
		_visionTexture.UpdateTexture(_visionMatrix);
	}
	
}
